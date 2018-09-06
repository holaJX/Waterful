using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Waterful.Wechat.ViewModels;
using Waterful.Wechat.Extensions;
using Waterful.Core;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Waterful.Core.Models;
using Microsoft.EntityFrameworkCore;
using Waterful.Core.Enums;
using Waterful.Wechat.Options;
using Microsoft.Extensions.Options;

namespace Waterful.Wechat.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork _unitOfWork;
        private readonly IdGenerationService _IdGenerationService;
        private readonly ProductOption _productOption;

        public OrderController(ILogger<OrderController> logger, UnitOfWork unitOfWork, IdGenerationService idGenerationService, IOptionsSnapshot<ProductOption> productOption)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _IdGenerationService = idGenerationService;
            _productOption = productOption.Value;
        }

        #region 检查状态
        /// <summary>
        /// 检查状态，是否可以购买
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckStatus(OrderBuyVM model)
        {
            AjaxResult dto = new AjaxResult();
            try
            {
                if (!ModelState.IsValid)
                {
                    dto.msg = ModelState.ErrorMessages();
                }
                else
                {
                    var cid = User.Identities.DefaultCustomerId();
                    var product = _unitOfWork.ProductRepository.SingleOrDefault(x => x.Id == model.ProductId && x.Status == 1);

                    if (product == null)
                    {
                        dto.msg = "产品信息无效。";
                    }
                    if (product.Storage == 0)
                    {
                        dto.msg = "产品库存不足。";
                    }
                    else
                    {
                        if (_unitOfWork.OrderRepository.Count(x => x.CustomerId == cid && x.Status == 0 && x.ParentId == 0) >= _productOption.OrderLimit)
                        {
                            dto.msg = "您有订单尚未支付，请勿重复下单。";
                            return Ok(dto);
                        }

                        dto.err = 1;
                        dto.msg = "下单成功。";
                    }
                }
            }
            catch (Exception ex)
            {
                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(CheckStatus), ex);
            }
            return Ok(dto);
        }
        #endregion

        #region 支付
        /// <summary>
        /// 支付详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult PayDetails(OrderBuyVM model)
        {
            if (model.Id == null && model.ProductId == null)
            {
                return NotFound();
            }
            try
            {
                var vm = new OrderDetailsVM();
                var isSecondYear = false;
                var isHalfYear = false;

                //新订单
                if (model.Id == null)
                {
                    var product = _unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == model.ProductId && x.Status == 1);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    var order = BuildOrderFromProduct(product, model.Times);
                    var list = new OrderItemVM().BindList(order.OrderItems.ToList());
                    vm.Bind(order);
                    vm.OrderItems = list;
                }
                //续费订单
                else
                {
                    if (!model.IsRenew)
                    {
                        return NotFound();
                    }

                    vm = GetOrderDetailsVM(model.Id);
                    if (vm == null)
                    {
                        return NotFound();
                    }

                    var product = vm.OrderItems.FirstOrDefault().Product;
                    if (product == null)
                    {
                        return NotFound();
                    }

                    if (vm.OrderType == 1)//购买
                    {
                        vm.Total = vm.FilterPrice;
                    }
                    else if (vm.OrderType == 2)//租用
                    {
                        if (vm.PayTime == null || vm.NextPayTime == null)
                        {
                            return NotFound();
                        }

                        vm.Total = product.Price - product.InstallFee - product.DepositAmount;

                        isHalfYear = IsHalfYear(vm.Month, vm.PayTime.Value, vm.NextPayTime.Value);
                        isSecondYear = IsSecondYear(vm.Month, vm.PayTime.Value, vm.NextPayTime.Value);

                        if (isHalfYear)//半年付
                        {
                            vm.Total = vm.Total / 2;
                        }
                        if (isSecondYear)//[活动]第二年优惠
                        {
                            var level = vm.OrderItems.SingleOrDefault().Product.Level;
                            vm.Total = vm.Total - _productOption.SecondYearDiscount[level - 1];
                        }
                    }
                }

                ViewData["IsRenew"] = model.IsRenew;
                ViewData["IsSecondYear"] = isSecondYear;
                ViewData["IsHalfYear"] = isHalfYear;
                ViewData["SecondYearDiscount"] = _productOption.SecondYearDiscount;

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(PayDetails), ex);
                return NotFound();
            }
        }


        /// <summary>
        /// 验证线下券
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IActionResult GetOnLineCoupon(string no)
        {
            AjaxResult<Coupon> dto = new AjaxResult<Coupon>();
            try
            {
                var time = DateTime.Now;
                var status = 1;
                var coupon = _unitOfWork.CouponRepository
                                .SingleOrDefault(x => x.CouponNo == no && x.ExpiryDate > time && x.CouponType == CouponEnum.OffLine && !x.Used && x.Status == status);

                if (coupon == null)
                {
                    dto.msg = "优惠券不存在或已过期。";
                }
                else
                {
                    dto.err = 1;
                    dto.data = coupon;
                    dto.msg = "优惠券验证成功。";
                }
            }
            catch (Exception ex)
            {
                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(GetOnLineCoupon), ex);
            }
            return Ok(dto);
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Pay(OrderPayVM model)
        {
            AjaxResult<int> dto = new AjaxResult<int>();
            try
            {
                if (!ModelState.IsValid)
                {
                    dto.msg = ModelState.ErrorMessages();
                }
                else
                {
                    #region 获取产品信息
                    var product = _unitOfWork.ProductRepository.SingleOrDefault(x => x.Id == model.ProductId && x.Status == 1);
                    if (product == null)
                    {
                        dto.msg = "产品信息无效。"; return Ok(dto);
                    }
                    else if (product.Storage == 0)
                    {
                        dto.msg = "产品库存不足。"; return Ok(dto);
                    }

                    var cid = User.Identities.DefaultCustomerId();

                    if (_unitOfWork.OrderRepository.Count(x => x.CustomerId == cid && x.Status == 0 && x.ParentId == 0) >= _productOption.OrderLimit)
                    {
                        dto.msg = "您有订单尚未支付，请勿重复下单。";
                        return Ok(dto);
                    }
                    #endregion

                    #region 获取订单信息
                    var order = BuildOrderFromProduct(product, model.ServiceNumber);

                    var parentOrder = new Order();
                    if (model.Action == OrderAction.Create)
                    {
                        if (product.CategoryId == CategoryEnum.ClearWater && product.PaymentType == PaymentEnum.Buy)
                        {
                            order.ServiceNumber++;//[活动]净水器购买多送一个滤芯
                            order.OrderItems.FirstOrDefault().FilterNumber++;
                        }
                    }
                    else if (model.Action == OrderAction.Renew)
                    {
                        parentOrder = _unitOfWork.OrderRepository.FirstOrDefault(x => x.Id == model.Id && !x.Close && x.Status == 1);
                        if (parentOrder == null)
                        {
                            dto.msg = "父订单信息错误，请刷新重试。";
                            return Ok(dto);
                        }
                        order.InstallAmount = 0;
                        order.DepositAmount = 0;
                        order.OrderItems.FirstOrDefault().InstallAmount = 0;
                        order.OrderItems.FirstOrDefault().DepositAmount = 0;
                    }
                    #endregion

                    #region 查询优惠券
                    var coupon = new Coupon();
                    if (model.CouponId != null)
                    {
                        coupon = _unitOfWork.CouponRepository.SingleOrDefault(x => x.Id == model.CouponId && x.ExpiryDate > DateTime.Now && !x.Used && x.Status == 1);

                        if (coupon == null)
                        {
                            dto.msg = "优惠券不存在或已使用，请刷新重试。";
                            return Ok(dto);
                        }

                        order.CouponNo = coupon.CouponNo;
                        switch (coupon.Type)
                        {
                            case CouponTypeEnum.Used: order.DiscountAmount = coupon.Discount; break;
                            case CouponTypeEnum.Feel: order.DiscountAmount = 0; break;
                            case CouponTypeEnum.FreeInstall: order.DiscountAmount = product.InstallFee; break;
                            case CouponTypeEnum.FreeDeposit: order.DiscountAmount = product.DepositAmount; break;
                            default: order.DiscountAmount = 0; break;
                        }
                    }
                    #endregion

                    #region 计算金额
                    if (model.Action == OrderAction.Create)
                    {
                        order.Month = model.Month;//购买为0，租用为6、12
                        var monthPrice = order.Month == 6 ? (product.Price - product.InstallFee - product.DepositAmount) / 2 : 0;
                        order.Total = order.Total - monthPrice;
                        order.Amount = order.Total - order.DiscountAmount;
                    }
                    else if (model.Action == OrderAction.Renew)
                    {
                        if (product.PaymentType == PaymentEnum.Buy)//购买
                        {
                            order.Total = order.FilterPrice * model.ServiceNumber;
                            order.Amount = order.Total - order.DiscountAmount;
                        }
                        else if (product.PaymentType == PaymentEnum.Rent)//租用
                        {
                            if (parentOrder.Id == 0 || parentOrder.Status != 1 || parentOrder.PayTime == null || parentOrder.NextPayTime == null)
                            {
                                dto.msg = "父订单信息错误，请刷新重试。";
                                return Ok(dto);
                            }

                            order.Total = product.Price - product.InstallFee - product.DepositAmount;
                            order.Month = 12;

                            var isHalfYear = IsHalfYear(parentOrder.Month, parentOrder.PayTime.Value, parentOrder.NextPayTime.Value);
                            var isSecondYear = IsSecondYear(parentOrder.Month, parentOrder.PayTime.Value, parentOrder.NextPayTime.Value);

                            order.ServiceNumber = product.Level == 3 ? 6 : 3;

                            if (isHalfYear)
                            {
                                order.Total = order.Total / 2;
                                order.Month = 6;
                                order.ServiceNumber = 0;
                            }
                            if (isSecondYear)
                            {
                                order.Total = order.Total - _productOption.SecondYearDiscount[product.Level - 1];
                            }

                            order.Amount = order.Total - order.DiscountAmount;                        
                            order.OrderItems.FirstOrDefault().FilterNumber = order.ServiceNumber;
                        }
                    }
                    order.OrderItems.FirstOrDefault().Amount = order.Total;

                    if (order.Amount != model.Amount)
                    {
                        dto.msg = "金额错误，请刷新重试。";
                        return Ok(dto);
                    }
                    #endregion

                    #region 创建订单
                    if (coupon.Id > 0 && coupon.Type == CouponTypeEnum.Feel)
                    {
                        order.Month += coupon.FeelTime;
                    }
                    order.OrderNo = _IdGenerationService.GenerateId().ToString();
                    order.ParentId = parentOrder.Id;
                    order.Street = model.Street;
                    order.Mobile = model.Mobile;
                    order.Name = model.Name;
                    order.CreateTime = DateTime.Now;
                    product.Storage--;//更新库存

                    _unitOfWork.OrderRepository.Insert(order, false);
                    _unitOfWork.ProductRepository.Update(product, false);
                    _unitOfWork.SaveChange();
                    #endregion

                    #region 更新优惠券
                    // 获取优惠券
                    if (coupon.Id != 0)
                    {
                        coupon.Used = true;
                        coupon.UpdateTime = DateTime.Now;
                        var couponUse = new CouponUse()
                        {
                            CouponId = model.CouponId.Value,
                            CreateTime = DateTime.Now,
                            CustomerId = User.Identities.DefaultCustomerId(),
                            OrderId = order.Id,
                            Status = 1
                        };

                        _unitOfWork.CouponRepository.Update(coupon, false);
                        _unitOfWork.CouponUseRepository.Insert(couponUse, false);
                        _unitOfWork.SaveChange();
                    }
                    #endregion

                    dto.data = order.Id;
                    dto.err = 1;
                    dto.msg = "成功。";
                }
            }
            catch (Exception ex)
            {
                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(Pay), ex);
            }
            return Ok(dto);
        }

        #endregion

        #region 我的订单
        /// <summary>
        /// 我的订单
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var cid = User.Identities.DefaultCustomerId();

            var orders = _unitOfWork.OrderRepository
                .Find(x => x.CustomerId == cid && x.OrderItems.Any() && x.ParentId == 0 && x.Status > -2)
                .OrderByDescending(o => o.CreateTime)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                .Include(x=>x.Aftersales)
                .ToList();

            return View(orders);
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var vm = GetOrderDetailsVM(id);
            if (vm == null)
            {
                return NotFound();
            }

            var aftersales = _unitOfWork.AftersaleRepository.Find(x => x.OrderId == id).OrderByDescending(o=>o.ServiceTime).ToList();
            if (aftersales.Any())
            {
                vm.Aftersales = new AftersaleVM().BindList(aftersales);
            }

            var isHalfYear = false;
            var isSecondYear = false;

            if (vm.OrderType == 2 && vm.Status == 1)
            {
                if (vm.PayTime == null || vm.NextPayTime == null)
                {
                    return NotFound();
                }

                isHalfYear = IsHalfYear(vm.Month, vm.PayTime.Value, vm.NextPayTime.Value);
                isSecondYear = IsSecondYear(vm.Month, vm.PayTime.Value, vm.NextPayTime.Value);
            }

            ViewData["IsHalfYear"] = isHalfYear;
            ViewData["IsSecondYear"] = isSecondYear;
            ViewData["SecondYearDiscount"] = _productOption.SecondYearDiscount;

            return View(vm);
        }
        #endregion

        #region 取消订单
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CancelOrder(int? id)
        {
            AjaxResult dto = new AjaxResult();

            try
            {
                if (id == null)
                {
                    dto.msg = "订单信息错误";
                    return Ok(dto);
                }

                var cid = User.Identities.DefaultCustomerId();
                var order = _unitOfWork.OrderRepository.SingleOrDefault(x => x.Id == id && x.CustomerId == cid && x.Status == 0);

                if (order == null)
                {
                    dto.msg = "未找到订单信息。";
                    return Ok(dto);
                }

                var item = _unitOfWork.OrderItemRepository.Find(x => x.OrderId == id).Include(x => x.Product).FirstOrDefault();
                if (item == null || item.Product == null)
                {
                    dto.msg = "未找到产品信息。";
                    return Ok(dto);
                }
                DateTime dt = DateTime.Now;
                //返回券
                if (!string.IsNullOrWhiteSpace(order.CouponNo))
                {

                    var coupon = _unitOfWork.CouponRepository.SingleOrDefault(e => e.CouponNo == order.CouponNo);
                    if (coupon != null && coupon.Used)
                    {
                        coupon.Used = false;
                        coupon.UpdateTime = dt;
                        _unitOfWork.CouponRepository.Update(coupon, false);
                    }

                }
                order.Status = -1;
                order.UpdateTime = dt;
                item.Product.Storage++;

                _unitOfWork.OrderRepository.Update(order, false);
                _unitOfWork.ProductRepository.Update(item.Product, false);
                _unitOfWork.SaveChange();

                dto.err = 1;
                dto.msg = "成功。";
            }
            catch (Exception ex)
            {
                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(Pay), ex);
            }
            return Ok(dto);
        }
        #endregion

        #region 关闭订单
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CloseOrder(int? id)
        {
            AjaxResult dto = new AjaxResult();

            try
            {
                if (id == null)
                {
                    dto.msg = "订单信息错误";
                    return Ok(dto);
                }

                var cid = User.Identities.DefaultCustomerId();
                var order = _unitOfWork.OrderRepository.SingleOrDefault(x => x.Id == id && x.CustomerId == cid && x.Status == 1);

                if (order == null)
                {
                    dto.msg = "未找到订单信息。";
                    return Ok(dto);
                }

                order.Close = true;
                order.CloseTime = DateTime.Now;
                order.UpdateTime = DateTime.Now;
                _unitOfWork.OrderRepository.Update(order);

                dto.err = 1;
                dto.msg = "成功。";
            }
            catch (Exception ex)
            {
                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(Pay), ex);
            }
            return Ok(dto);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取订单详细ViewModel
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        private OrderDetailsVM GetOrderDetailsVM(int? id)
        {
            var cid = User.Identities.DefaultCustomerId();
            var order = _unitOfWork.OrderRepository.SingleOrDefault(x => x.Id == id && x.CustomerId == cid);

            if (order == null)
            {
                return null;
            }

            // 获取订单详情，如果是子订单返回父订单详情
            if (order.ParentId != 0)
            {
                order = _unitOfWork.OrderRepository.SingleOrDefault(x => x.Id == order.ParentId && x.CustomerId == cid);
            }

            var orderItems = _unitOfWork.OrderItemRepository
                              .Find(x => x.OrderId == order.Id)
                              .Include(x => x.Product).ToList();

            if (order == null || !orderItems.Any())
            {
                return null;
            }

            var vm = new OrderDetailsVM();
            vm.Bind(order);
            vm.OrderItems = new OrderItemVM().BindList(orderItems);
            vm.Address = new AddressVM();
            vm.Aftersales = new List<AftersaleVM>();

            return vm;
        }

        /// <summary>
        /// 获取收货地址ViewModel
        /// </summary>
        /// <returns></returns>
        private AddressVM GetAddressVM()
        {
            var cid = User.Identities.DefaultCustomerId();
            var addressVM = new AddressVM();
            var address = _unitOfWork.AddressRepository
                            .Find(x => x.Status == 1 && x.CusomerId == cid)
                            .OrderByDescending(o => o.Sort).FirstOrDefault();

            if (address != null)
            {
                addressVM.Id = address.Id;
                addressVM.Mobile = address.Mobile;
                addressVM.Name = address.Name;
                addressVM.Display = address.Display;
                addressVM.Sort = address.Sort;
                addressVM.Street = address.Street;
            }
            else
            {
                return null;
            }

            return addressVM;
        }

        /// <summary>
        /// 通过产品信息构造订单
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="serverTimes">服务次数</param>
        /// <returns></returns>
        private Order BuildOrderFromProduct(Product product, int serverTimes)
        {
            var cid = User.Identities.DefaultCustomerId();

            var orderItems = new OrderItem()
            {
                Amount = product.Price + product.FilterPrice * (serverTimes - 1),//产品购买的价格都包含一次滤芯
                CreateTime = DateTime.Now,
                DepositAmount = product.DepositAmount,
                FilterNumber = serverTimes,
                FilterPrice = product.FilterPrice,
                InstallAmount = product.InstallFee,
                Name = product.Name,
                ProductId = product.Id,
                Product = product
            };

            if (product.CategoryId == CategoryEnum.ClearWater && product.PaymentType == PaymentEnum.Rent)
            {
                orderItems.Amount = product.Price;//产品租用的价格
            }

            var items = new List<OrderItem>();
            items.Add(orderItems);

            var order = new Order()
            {
                CreateTime = DateTime.Now,
                CustomerId = cid,
                DepositAmount = items.Sum(i => i.DepositAmount),
                FilterPrice = product.FilterPrice,
                InstallAmount = items.Sum(i => i.InstallAmount),
                ServiceNumber = items.Sum(i => i.FilterNumber),
                Status = 0,
                Total = items.Sum(i => i.Amount),
                OrderItems = items,
                OrderType = (int)product.PaymentType
            };

            return order;
        }


        private bool IsHalfYear(int month, DateTime payTime, DateTime nextPayTime)
        {
            var result = false;

            if (month < 12)
            {
                var npt = payTime.AddMonths(month);
                result = nextPayTime.CompareTo(npt) == 0;
            }

            return result;
        }
        private bool IsSecondYear(int month, DateTime payTime, DateTime nextPayTime)
        {
            var result = false;
            var npt = payTime.AddMonths(month);

            if (month < 12)
            {
                npt = npt.AddMonths(6);
            }

            result = nextPayTime.CompareTo(npt) == 0;

            return result;
        }
        #endregion
        public IActionResult test()
        {
            return View();
        }
    }
}
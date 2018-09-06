using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Waterful.Core;
using Waterful.Core.Models;
using X.PagedList;
using Waterful.Back.ViewModels;

namespace Waterful.Back.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderController : LoginControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public OrderController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="mobile"></param>
        /// <param name="name"></param>
        /// <param name="id">售卖订单 1 租用订单 2  续费售卖 5 续费租用 6</param>
        /// <param name="status"></param>
        /// <param name="p"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IActionResult Index(string begin, string end, string mobile, string name, int id = 0, int status = -10, int p = 1, int pageSize = 10)
        {
            p = p < 1 ? 1 : p;
            int count = 0;
            IQueryable<Order> result;

            if (!string.IsNullOrWhiteSpace(begin) && !string.IsNullOrWhiteSpace(end))
            {
                ViewData["begin"] = begin;
                ViewData["end"] = end;
            }
            ViewData["mobile"] = mobile;
            ViewData["name"] = name;
            ViewData["status"] = status;
            ViewData["type"] = id;

            result = _unitOfWork.OrderRepository.SearchList(p, pageSize, out count, begin, end, mobile, name, status, id);

            var pageList = new StaticPagedList<Order>(result, p, pageSize, count);

            return View(pageList);
        }

        public ActionResult Details(int id, string isRemark)
        {
            if (id < 1)
            {
                return NotFound();
            }
            var order = _unitOfWork.OrderRepository.FindInclude(i => i.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            var orderItem = order.OrderItems.FirstOrDefault();
            if (orderItem != null)
            {
                ViewData["OrderItem"] = orderItem;
                //ViewData["Product"] = _unitOfWork.ProductRepository.FirstOrDefault(i => i.Id == orderItem.ProductId);
            }
            ViewData["isRemark"] = isRemark;
            ViewData["Customer"] = _unitOfWork.CustomerRepository.FirstOrDefault(i => i.Id == id);
            int count = 0;
            ViewData["Aftersales"] = _unitOfWork.AftersaleRepository.SearchList(1, 999, out count, i => i.OrderId == id);

            return View(order);
        }

        #region 添加编辑服务信息

        /// <summary>
        /// 添加服务信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CreateAftersale(int Id)
        {
            ViewData["Workers"] = _unitOfWork.WorkerRepository.GetAllList(i => i.Status > 0);
            if (Id < 1)
            {
                return NotFound();
            }
            var entity = new CreateAftersaleVM();
            entity.Id = Id;
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAftersale(CreateAftersaleVM model)
        {
            ViewData["Workers"] = _unitOfWork.WorkerRepository.GetAllList(i => i.Status > 0);
            if (ModelState.IsValid)
            {
                DateTime d = DateTime.MinValue;
                if (string.IsNullOrWhiteSpace(model.ServiceTime) || model.WorkerId < 1 || !DateTime.TryParse(model.ServiceTime, out d))
                {
                    ViewBag.ErrorInfo = "时间或安装人员必选。";
                    return View(model);
                }
                var order = _unitOfWork.OrderRepository.FirstOrDefault(m => m.Id == model.Id && m.Status > 0);
                if (order == null)
                {
                    ViewBag.ErrorInfo = "该订单不存在或未支付，请刷新列表页后重试。";
                    return View(model);
                }

                var serviceNumber = _unitOfWork.AftersaleRepository.Count(i => i.OrderId == model.Id && i.Status == 0);
                if (serviceNumber >= order.ServiceNumber)
                {
                    ViewBag.ErrorInfo = "该订单剩余服务次数已用尽，无法添加上门服务信息";
                    return View(model);
                }
                var entity = new Aftersale();
                entity.CustomerId = order.CustomerId;
                entity.OrderId = model.Id;
                entity.WorkerId = model.WorkerId;
                entity.ServiceTime = d;
                entity.Remark = model.Remark;

                _unitOfWork.AftersaleRepository.Insert(entity);

                return RedirectToAction("Details", new { Id = model.Id });
            }
            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return View(model);
        }
        /// <summary>
        /// 编辑服务信息
        /// </summary>
        /// <param name="Id">OrderId</param>
        /// <returns></returns>
        public ActionResult EditAftersale(int Id, int AftersaleId)
        {
            if (Id < 1 || AftersaleId < 1)
            {
                return NotFound();
            }
            ViewData["Workers"] = _unitOfWork.WorkerRepository.GetAllList(i => i.Status > 0);
            var entity = _unitOfWork.AftersaleRepository.FirstOrDefault(m => m.Id == AftersaleId && m.Status > -1);
            if (entity == null)
            {
                return NotFound();
            }
            var vm = new CreateAftersaleVM();
            vm.Id = Id;
            vm.AftersaleId = AftersaleId;
            vm.WorkerId = entity.WorkerId;
            vm.ServiceTime = entity.ServiceTime.ToString();
            vm.Remark = entity.Remark;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAftersale(CreateAftersaleVM model)
        {
            ViewData["Workers"] = _unitOfWork.WorkerRepository.GetAllList(i => i.Status > 0);
            if (ModelState.IsValid)
            {
                DateTime d = DateTime.MinValue;
                if (string.IsNullOrWhiteSpace(model.ServiceTime) || model.WorkerId < 1 || !DateTime.TryParse(model.ServiceTime, out d))
                {
                    ViewBag.ErrorInfo = "时间或安装人员必选。";
                    return View(model);
                }
                var order = _unitOfWork.OrderRepository.FirstOrDefault(m => m.Id == model.Id && m.Status > 0);
                if (order == null)
                {
                    ViewBag.ErrorInfo = "该订单不存在或未支付，请刷新列表页后重试。";
                    return View(model);
                }
                var entity = _unitOfWork.AftersaleRepository.FirstOrDefault(m => m.Id == model.AftersaleId && m.Status > -1);
                if (entity == null)
                {
                    ViewBag.ErrorInfo = "服务信息不存在，请刷新列表页后重试。";
                    return View(model);
                }

                entity.ServiceTime = d;
                entity.WorkerId = model.WorkerId;
                entity.Remark = model.Remark;
                entity.UpdateTime = DateTime.Now;

                _unitOfWork.AftersaleRepository.Update(entity);

                return RedirectToAction("Details", new { Id = model.Id });
            }
            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return View(model);
        }
        #endregion
        /// <summary>
        /// 详情页备注更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Remark(int id, string Remark)
        {
            //if (ModelState.IsValid)
            //{
            var entity = _unitOfWork.OrderRepository.FirstOrDefault(m => m.Id == id && m.Status > -1);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Remark = Remark;
            _unitOfWork.OrderRepository.Update(entity);

            return RedirectToAction("Details", new { Id = id, isRemark = 1 });
        }
        /// <summary>
        /// 发送服务通知短信
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Sms(int id)
        {
            //    CommentVM vm = new CommentVM();
            AjaxResult dto = new AjaxResult();
            if (id < 1)
            {
                dto.msg = "参数错误";
                return Json(dto);
            }
            var entity = _unitOfWork.AftersaleRepository.FirstOrDefault(m => m.Id == id && m.Status > -1);
            if (entity == null)
            {
                dto.msg = "上门服务信息不存在";
                return Json(dto);
            }
            var cus = _unitOfWork.CustomerRepository.FirstOrDefault(i => i.Id == entity.CustomerId && i.Status > 0);
            if (string.IsNullOrWhiteSpace(cus.Mobile))
            {
                dto.msg = "发送失败，该用户未绑定手机号";
                return Json(dto);
            }
            //发送短信
            var time = entity.ServiceTime.ToString("MM月dd日");
            MessageService sms = new MessageService();
            sms.SendSmsMsg(cus.Mobile, time);
            dto.msg = $"发送成功,手机号{cus.Mobile},时间{time}";
            dto.err = 1;
            return Json(dto);
        }
        #region 关闭/取消订单
        public ActionResult Cancel(int id)
        {
            var entity = _unitOfWork.OrderRepository.FirstOrDefault(m => m.Id == id);
            return View(entity);
        }

        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            var entity = _unitOfWork.OrderRepository.FirstOrDefault(m => m.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            if (entity.Status == 0)
            {
                var dt = DateTime.Now;
                entity.Status = -1;
                entity.UpdateTime = dt;
                _unitOfWork.OrderRepository.Update(entity);
            }
            else
            {

            }
            int type = 0;
            if (entity.OrderType == 1)
            {
                if (entity.ParentId == 0)
                {
                    type = 1;
                }
                else
                {
                    type = 5;
                }
            }
            else if (entity.OrderType == 2)
            {
                if (entity.ParentId == 0)
                {
                    type = 2;
                }
                else
                {
                    type = 6;
                }
            }
            return RedirectToAction("Index", new { id = type });
        }
        public ActionResult Delete(int id)
        {
            var entity = _unitOfWork.OrderRepository.FirstOrDefault(m => m.Id == id);
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var entity = _unitOfWork.OrderRepository.FirstOrDefault(m => m.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            if (entity.Status == -1)
            {
                var dt = DateTime.Now;
                entity.Status = -2;
                entity.UpdateTime = dt;
                _unitOfWork.OrderRepository.Update(entity);
            }
            else
            {

            }
            int type = 0;
            if (entity.OrderType == 1)
            {
                if (entity.ParentId == 0)
                {
                    type = 1;
                }
                else
                {
                    type = 5;
                }
            }
            else if (entity.OrderType == 2)
            {
                if (entity.ParentId == 0)
                {
                    type = 2;
                }
                else
                {
                    type = 6;
                }
            }
            return RedirectToAction("Index", new { id = type });
        }
        #endregion

        //#if DEBUG

        //        public IActionResult Create()
        //        {
        //            return View();
        //        }

        //        // POST: Order/Create
        //        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> Create([Bind("PayNotify,PayTime,CustomerId,OrderNo,Channel,Total,Amount,DiscountAmount,DepositAmount,InstallAmount,Name,Mobile,Street,CouponNo,Status,ParentId,FilterPrice,ServiceNumber,Close,CloseTime,Remark,CreateTime,UpdateTime,NextPayTime,Month,Timestamp,Id")] Order order)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                _context.Add(order);
        //                await _context.SaveChangesAsync();
        //                return RedirectToAction("Index");
        //            }
        //            return View(order);
        //        }

        //        // GET: Order/Edit/5
        //        public async Task<IActionResult> Edit(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
        //            if (order == null)
        //            {
        //                return NotFound();
        //            }
        //            return View(order);
        //        }

        //        // POST: Order/Edit/5
        //        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> Edit(int id, [Bind("PayNotify,PayTime,CustomerId,OrderNo,Channel,Total,Amount,DiscountAmount,DepositAmount,InstallAmount,Name,Mobile,Street,CouponNo,Status,ParentId,FilterPrice,ServiceNumber,Close,CloseTime,Remark,CreateTime,UpdateTime,NextPayTime,Month,Timestamp,Id")] Order order)
        //        {
        //            if (id != order.Id)
        //            {
        //                return NotFound();
        //            }

        //            if (ModelState.IsValid)
        //            {
        //                try
        //                {
        //                    _context.Update(order);
        //                    await _context.SaveChangesAsync();
        //                }
        //                catch (DbUpdateConcurrencyException)
        //                {
        //                    if (!OrderExists(order.Id))
        //                    {
        //                        return NotFound();
        //                    }
        //                    else
        //                    {
        //                        throw;
        //                    }
        //                }
        //                return RedirectToAction("Index");
        //            }
        //            return View(order);
        //        }

        //        // GET: Order/Delete/5
        //        public async Task<IActionResult> Delete(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            var order = await _context.Orders
        //                .SingleOrDefaultAsync(m => m.Id == id);
        //            if (order == null)
        //            {
        //                return NotFound();
        //            }

        //            return View(order);
        //        }

        //        // POST: Order/Delete/5
        //        [HttpPost, ActionName("Delete")]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> DeleteConfirmed(int id)
        //        {
        //            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
        //            _context.Orders.Remove(order);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction("Index");
        //        }

        //        private bool OrderExists(int id)
        //        {
        //            return _context.Orders.Any(e => e.Id == id);
        //        }
        //#endif
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Waterful.Core;
using Waterful.Core.Enums;
using Waterful.Core.Models;

namespace Waterful.Wechat.ViewComponents
{
    public class CouponsViewComponent : ViewComponent
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork _unitOfWork;
        private readonly IdGenerationService _IdGenerationService;


        public CouponsViewComponent(ILogger<CouponsViewComponent> logger, UnitOfWork unitOfWork, IdGenerationService idGenerationService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _IdGenerationService = idGenerationService;
        }

        /// <summary>
        /// 优惠券组件
        /// </summary>
        /// <param name="cid">用户id</param>
        /// <param name="payType">1买2租</param>
        /// <param name="isRenew">是否续费订单</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int cid, PaymentEnum payType, bool isRenew)
        {
            var items = await GetItemsAsync(cid, payType, isRenew);

            if (!items.Any())
            {
                items = new List<Coupon>();
            }

            return View(items);
        }

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="cid">用户id</param>
        /// <param name="payType">1买2租</param>
        /// <param name="isRenew">是否续费订单</param>
        /// <returns></returns>
        private Task<List<Coupon>> GetItemsAsync(int cid, PaymentEnum payType, bool isRenew)
        {
            var query = _unitOfWork.CouponRepository.Find(x => x.CustomerId == cid && x.ExpiryDate > DateTime.Now &&
                                                         x.CouponType == CouponEnum.OnLine && !x.Used && x.Status == 1 && x.Type != CouponTypeEnum.Free);
            if (payType == PaymentEnum.Buy)
            {
                query = query.Where(x => x.Type != CouponTypeEnum.Feel && x.Type != CouponTypeEnum.FreeDeposit);
            }

            if (isRenew)
            {
                query = query.Where(x => x.Type != CouponTypeEnum.FreeInstall && x.Type != CouponTypeEnum.FreeDeposit);
            }

            return query.ToListAsync();
        }
    }
}

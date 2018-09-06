using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Waterful.Core.Enums;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 优惠券表
    /// </summary>
    public class Coupon : Entity
    {
        /// <summary>
        ///兑换码
        /// </summary>
        [MaxLength(30)]
        public string CouponNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpiryDate { get; set; }
        /// <summary>
        /// 券类型 1=线上  2=线下
        /// </summary>

        public CouponEnum CouponType { get; set; }
        /// <summary>
        /// 代金券 =1 体验券= 2  礼品券=3 免安装费券=4  免押金券 =5
        /// </summary>
        public CouponTypeEnum Type { get; set; }
        /// <summary>
        /// 是否被使用
        /// </summary>
        public bool Used { get; set; }
        /// <summary>
        /// 默认为0 
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 抵扣金额(默认0)
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// 体验时长（月）
        /// </summary>
        public int FeelTime { get; set; }
        /// <summary>
        /// 状态 0 关闭 1正常
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

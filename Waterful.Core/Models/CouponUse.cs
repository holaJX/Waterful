using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 优惠券认领表
    /// </summary>
    public class CouponUse : Entity
    {
        public int CouponId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
       
        /// <summary>
        /// 状态 0 未使用 1 已使用 -1 作废
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

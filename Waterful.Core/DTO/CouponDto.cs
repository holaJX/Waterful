using System;
using System.Collections.Generic;
using System.Text;
using Waterful.Core.Enums;

namespace Waterful.Core.DTO
{
    public class CouponDto
    {
        /// <summary>
        ///  线上=1 线下=2
        /// </summary>

        public CouponEnum CouponType { get; set; }

        /// <summary>
        /// 抵扣券/体验/免费
        /// </summary>
        public CouponTypeEnum Type{ get; set; }

        public string Name { get; set; }

        public string CouponNo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Waterful.Core.Enums
{
    /// <summary>
    /// 代金券 =1 体验券= 2  礼品券=3 免安装费券=4 免押金=5
    /// </summary>
    public enum CouponTypeEnum
    {
        /// <summary>
        /// 代金券
        /// </summary>
        Used = 1,
        /// <summary>
        /// 体验券
        /// </summary>
        Feel = 2,

        /// <summary>
        /// (礼品券)
        /// </summary>
        Free = 3,
        /// <summary>
        /// 免安装费券
        /// </summary>
        FreeInstall = 4,

        /// <summary>
        /// 免押金
        /// </summary>
        FreeDeposit = 5,
    }
}

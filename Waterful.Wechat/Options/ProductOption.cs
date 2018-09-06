using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Wechat.Options
{
    public class ProductOption
    {
        /// <summary>
        /// 前台地址
        /// </summary>
        public int OrderLimit { get; set; }
        /// <summary>
        /// 前台文件绝对路径
        /// </summary>
        public int[] SecondYearDiscount { get; set; }
        /// <summary>
        /// 秘钥(暂未启用)
        /// </summary>
        public string[] LevelTitle { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Wechat.ViewModels
{
    public class AddressVM
    {

        public int Id { get; set; }
        public long Sort { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 区域编号
        /// </summary>
        public string AreaId { get; set; }
        /// <summary>
        /// 详细地址（区域+填写地址 ）
        /// </summary>
        public string Display { get; set; }
    }
}

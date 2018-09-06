using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 收货地址
    /// </summary>
    public class Address : Entity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int CusomerId { get; set; }
        /// <summary>
        /// 收货姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 收货电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 区域编号
        /// </summary>
        public string AreaId { get; set; }
        /// <summary>
        /// 详细地址（区域+填写地址 ）
        /// </summary>
        public string Display { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 默认地址标记(更新最大时间 判断排序和默认地址)
        /// </summary>
        public long Sort { get; set; }
        /// <summary>
        /// 状态 0删除  1正常  
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public virtual Customer Customer { get; set; }
    }
}

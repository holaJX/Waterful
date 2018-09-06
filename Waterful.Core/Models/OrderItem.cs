using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 订单明细表
    /// </summary>
    public class OrderItem : Entity
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 产品类型（全部=0、部件=1）
        /// </summary>
        public int ProductType { get; set; }
        ///// <summary>
        ///// 抵扣
        ///// </summary>
        //public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal DepositAmount { get; set; }
        /// <summary>
        /// 滤芯价格
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal FilterPrice { get; set; }
        /// <summary>
        /// 滤芯次数
        /// </summary>
        public int FilterNumber{ get; set; }
        /// <summary>
        /// 安装费
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal InstallAmount { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }
    }
}

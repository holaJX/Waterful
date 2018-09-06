using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 佣金表
    /// </summary>
  public  class Commission :Entity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// 提成比例
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal Rate { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 0待结算、1已结算
        /// </summary>
        public int Status { get; set; }

        public DateTime? UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

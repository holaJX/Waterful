using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 订单表
    /// </summary>
    public class Order : Entity
    {
        #region 回调
        public string PayNotify { get; set; }
        public DateTime? PayTime { get; set; }
        #endregion
        /// <summary>
        /// 用户Id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 支付渠道 0 微信  默认 1
        /// </summary>
        public int Channel { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal Total { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 抵扣金额
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal DepositAmount { get; set; }
        /// <summary>
        /// 安装费
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal InstallAmount { get; set; }
        /// <summary>
        /// 收货姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 收货电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 优惠券编号
        /// </summary>
        public string CouponNo { get; set; }
        /// <summary>
        /// 状态 -2=删除 -1=订单取消 0=未支付(创建) 1=已支付  2=完成
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 售卖订单001 1 租用订单010 2
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        ///户级订单id默认为0（续费使用）
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 滤芯价格
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal FilterPrice { get; set; }
        /// <summary>
        ///后续服务次数
        /// </summary>
        public int ServiceNumber { get; set; }
        /// <summary>
        /// 关闭
        /// </summary>
        public bool Close { get; set; }
        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseTime { get; set; }
        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 下次付款时间（根据半年付、年付计算出来）
        /// </summary>
        public DateTime? NextPayTime { get; set; }
        //默认为0，半年付为6，年付为12
        public int Month { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Aftersale> Aftersales { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Waterful.Core.Models;

namespace Waterful.Wechat.ViewModels
{

    public enum OrderAction
    {
        Create,
        Update,
        Renew
    }
    public class OrderBuyVM
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 是否是续费
        /// </summary>
        public bool IsRenew { get; set; }
        /// <summary>
        /// 产品id
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// 服务次数
        /// </summary>
        [Required(ErrorMessage = "服务次数错误")]
        [Range(1, int.MaxValue)]
        public int Times { get; set; }
    }


    public class OrderPayVM
    {
        public OrderAction Action { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 产品id
        /// </summary>
        public int? ProductId { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        [Required(ErrorMessage = "订单金额错误")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 优惠券Id
        /// </summary>
        public int? CouponId { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        [Required(ErrorMessage = "收货人不能为空")]
        public string Name { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        [Required(ErrorMessage = "收货人电话不能为空")]
        public string Mobile { get; set; }
        /// <summary>
        /// 收货人地址
        /// </summary>
        [Required(ErrorMessage = "收货人地址不能为空")]
        public string Street { get; set; }
        /// <summary>
        /// 默认为0，半年付为6，年付为12
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        ///滤芯服务次数
        /// </summary>
        public int ServiceNumber { get; set; }
    }


    public class OrderDetailsVM
    {
        public void Bind(Order entity)
        {
            Id = entity.Id;
            Total = entity.Total;
            DepositAmount = entity.DepositAmount;
            InstallAmount = entity.InstallAmount;
            CreateTime = entity.CreateTime;
            OrderNo = entity.OrderNo;
            Month = entity.Month;
            Status = entity.Status;
            NextPayTime = entity.NextPayTime;
            Amount = entity.Amount;
            ServiceNumber = entity.ServiceNumber;
            Close = entity.Close;
            FilterPrice = entity.FilterPrice;
            OrderType = entity.OrderType;
            PayTime = entity.PayTime;
            Name = entity.Name;
            Mobile = entity.Mobile;
            Street = entity.Street;
        }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int? Id { get; set; }

        //#region 回调
        //public string PayNotify { get; set; }
        public DateTime? NextPayTime { get; set; }

        public DateTime? PayTime { get; set; }

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

        //#endregion
        ///// <summary>
        ///// 用户Id
        ///// </summary>
        //public int CustomerId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        ///// <summary>
        ///// 支付渠道 0 微信  默认 1
        ///// </summary>
        //public int Channel { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal Amount { get; set; }
        ///// <summary>
        ///// 抵扣金额
        ///// </summary>
        //public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal DepositAmount { get; set; }
        /// <summary>
        /// 安装费
        /// </summary>
        public decimal InstallAmount { get; set; }

        ///// <summary>
        ///// 优惠券编号
        ///// </summary>
        //public string CouponNo { get; set; }
        /// <summary>
        /// 状态 0= 创建 1  =已支付  失败= 2
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 售卖订单 1 租用订单 2
        /// </summary>
        public int OrderType { get; set; }
        ///// <summary>
        /////户级订单id默认为0（续费使用）
        ///// </summary>
        //public int ParentId { get; set; }

        /// <summary>
        /// 滤芯价格
        /// </summary>
        public decimal FilterPrice { get; set; }
        /// <summary>
        ///滤芯服务次数
        /// </summary>
        public int ServiceNumber { get; set; }
        /// <summary>
        /// 关闭
        /// </summary>
        public bool Close { get; set; }
        ///// <summary>
        ///// 关闭时间
        ///// </summary>
        //public DateTime? CloseTime { get; set; }
        //public string Remark { get; set; }

        public DateTime CreateTime { get; set; }
        //public DateTime UpdateTime { get; set; }
        ///// <summary>
        ///// 下次付款时间（根据半年付、年付计算出来）
        ///// </summary>
        //public DateTime? NextPayTime { get; set; }
        //默认为0，半年付为6，年付为12
        public int Month { get; set; }
        public virtual ICollection<OrderItemVM> OrderItems { get; set; }
        public virtual ICollection<AftersaleVM> Aftersales { get; set; }

        public virtual AddressVM Address { get; set; }
    }

    public class OrderItemVM
    {
        public List<OrderItemVM> BindList(List<OrderItem> list)
        {
            var result = new List<OrderItemVM>();
            foreach (var entity in list)
            {
                result.Add(new OrderItemVM()
                {
                    Amount = entity.Amount,
                    FilterPrice = entity.FilterPrice,
                    FilterNumber = entity.FilterNumber,
                    InstallAmount = entity.InstallAmount,
                    Product = new ProductDetailsVM(entity.Product)
                });
            }
            return result;
        }

        ///// <summary>
        ///// 订单Id
        ///// </summary>
        //public int OrderId { get; set; }
        ///// <summary>
        ///// 产品Id
        ///// </summary>
        //public int ProductId { get; set; }
        ///// <summary>
        ///// 产品名称
        ///// </summary>
        //public string Name { get; set; }
        ///// <summary>
        ///// 产品类型（全部=0、部件=1）
        ///// </summary>
        //public int ProductType { get; set; }
        /////// <summary>
        /////// 抵扣
        /////// </summary>
        ////public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Amount { get; set; }
        ///// <summary>
        ///// 押金
        ///// </summary>
        //[Column(TypeName = "decimal(65, 2)")]
        //public decimal DepositAmount { get; set; }

        /// <summary>
        /// 滤芯价格
        /// </summary>
        public decimal FilterPrice { get; set; }

        /// <summary>
        /// 滤芯次数
        /// </summary>
        public int FilterNumber { get; set; }

        /// <summary>
        /// 安装费
        /// </summary>

        public decimal InstallAmount { get; set; }
        //public string Remark { get; set; }
        //public DateTime CreateTime { get; set; }

        public virtual ProductDetailsVM Product { get; set; }

        //public virtual Order Order { get; set; }
    }


    public class AftersaleVM
    {
        public List<AftersaleVM> BindList(List<Aftersale> list)
        {
            var result = new List<AftersaleVM>();
            foreach (var item in list)
            {
                result.Add(new AftersaleVM()
                {
                    Id = item.Id,
                    CreateTime = item.CreateTime,
                    CustomerId = item.CustomerId,
                    OrderId = item.OrderId,
                    ServiceTime = item.ServiceTime,
                    Status = item.Status,
                    UpdateTime = item.UpdateTime,
                    WorkerId = item.WorkerId
                });
            }
            return result;
        }

        public int Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        public int WorkerId { get; set; }
        /// <summary>
        /// 服务时间
        /// </summary>
        public DateTime ServiceTime { get; set; }


        /// <summary>
        /// 服务状态,0=创建服务(后台派遣),1=已服务(用户点击确认服务) 2= 完成(评价后状态)
        /// </summary>
        public int Status { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

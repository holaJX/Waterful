using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waterful.Core.Enums;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 产品表
    /// </summary>
    public class Product : Entity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ProductNo { get; set; }
        /// <summary>
        /// 产品级别（一般、尊贵、顶级）
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 轮播图片
        /// </summary>
        public string Banner { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        public string VideoSrc { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int Storage { get; set; }
        /// <summary>
        /// 现价
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal Price { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal DepositAmount { get; set; }
        /// <summary>
        /// 安装费
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal InstallFee { get; set; }
        /// <summary>
        /// 滤芯价格
        /// </summary>
        [Column(TypeName = "decimal(65, 2)")]
        public decimal FilterPrice { get; set; }

        /// <summary>
        /// 净水器系统=1、饮水器=2、沐浴器=3
        /// </summary>
        public CategoryEnum CategoryId { get; set; }
        /// <summary>
        /// 交易方式1买 2租
        /// </summary>
        public PaymentEnum PaymentType { get; set; }
        /// <summary>
        /// 概要
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 服务介绍
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 状态,-1=删除,0=下架,1=销售
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 产品描述图片
        /// </summary>
        public string DescImg { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

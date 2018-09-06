using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waterful.Core.Enums;
using Waterful.Core.Models;

namespace Waterful.Wechat.ViewModels
{
    public class ProductListVM
    {
        /// <summary>
        /// 主键:净水器-买
        /// </summary>
        public int ClearBuyId { get; set; }

        /// <summary>
        /// 主键:净水器-租
        /// </summary>
        public int ClearRentId { get; set; }

        /// <summary>
        /// 主键:饮水器
        /// </summary>
        public int DrinkId { get; set; }

        /// <summary>
        /// 主键:沐浴器
        /// </summary>
        public int ShowerId { get; set; }

    }

    public class ProductDetailsVM
    {
        public ProductDetailsVM(Product entity)
        {
            Id = entity.Id;
            ImageUrl = entity.ImageUrl;
            Banner = entity.Banner;
            VideoSrc = entity.VideoSrc;
            Price = entity.Price;
            OriginalPrice = entity.OriginalPrice;
            FilterPrice = entity.FilterPrice;
            CategoryId = entity.CategoryId;
            PaymentType = entity.PaymentType;
            Name = entity.Name;
            Summary = entity.Summary;
            Description = entity.Description;
            Level = entity.Level;
            InstallFee = entity.InstallFee;
            DepositAmount = entity.DepositAmount;
            Service = entity.Service;
            DescImg = entity.DescImg;
        }

        public int Id { get; set; }
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
        /// 产品描述图片
        /// </summary>
        public string DescImg { get; set; }

        /// <summary>
        /// 轮播图片
        /// </summary>
        public string Banner { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        public string VideoSrc { get; set; }

        /// <summary>
        /// 现价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal DepositAmount { get; set; }
        /// <summary>
        /// 安装费
        /// </summary>
        public decimal InstallFee { get; set; }
        /// <summary>
        /// 滤芯价格
        /// </summary>
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
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 服务介绍
        /// </summary>
        public string Service { get; set; }
    }

    public class ProductServiceVM
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 产品级别（一般、尊贵、顶级）
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 现价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 滤芯价格
        /// </summary>
        public decimal FilterPrice { get; set; }

        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 概要
        /// </summary>
        public string Summary { get; set; }
    }
}

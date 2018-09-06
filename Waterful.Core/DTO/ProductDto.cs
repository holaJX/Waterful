using System;
using System.Collections.Generic;
using System.Text;
using Waterful.Core.Enums;

namespace Waterful.Core.DTO
{
    public class ProductDto
    {
        /// <summary>
        /// 净水器系统=1、饮水器=2、沐浴器=3
        /// </summary>

        public CategoryEnum CategoryId { get; set; }

        /// <summary>
        /// 交易方式（买=1、租赁=2）
        /// </summary>
        public PaymentEnum PaymentType { get; set; }

        public int level { get; set; }
    }
}

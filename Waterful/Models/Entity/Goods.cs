using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mincms.Models.Entity
{
    //产品 一对一 产品高度 重量 分类多对多
    //关联分类Categorie Attribute Edit Specification Attribute Details
    [Table("Goods")]
    public class Goods
    {
        public Goods()
        {
            Hits = 0;
            Sort = 0;
            AddTime = DateTime.Now;
            State = StateEnum.发布;
        }
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        [Required(ErrorMessage = "*必填")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字")]
        public string Title { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [DataType(DataType.Html)]
        public string Content { get; set; }
        [Display(Name = "属性")]
        [DataType(DataType.Html)]
        public string Attr { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        [Display(Name = "链接")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字符")]
        public string Url { get; set; }
        /// <summary>
        /// 录入者
        /// </summary>
        [Display(Name = "录入者")]
        [StringLength(50, ErrorMessage = "必须少于{0}个字")]
        public string Inputer { get; set; }
        /// <summary>
        /// 缩略图
        /// <remarks>图片至少上传1张（第一张不计图片空间容量），图片类型只能为gif,png,jpg,jpeg，且大小不超过500K</remarks>
        /// </summary>
        [Display(Name = "缩略图")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字符")]
        public string Thumb { get; set; }
        /// <summary>
        /// 摘要Intro/商品卖点
        /// </summary>
        [Display(Name = "摘要")]
        [StringLength(255, ErrorMessage = "必填少于{0}个字")]
        public string Description { get; set; }
        [Display(Name = "原价")]
        public decimal Price { get; set; }
        [Display(Name = "促销价")]
        public decimal Promo { get; set; }
        [Display(Name = "库存")]
        public int Stock { get; set; }
        //[Display(Name = "积分")]
        //public int Credit { get; set; }
        //[Display(Name = "条形码")]
        //public string BarCode { get; set; }
        [Display(Name = "货号")]
        public string SN { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        [Display(Name = "关键词")]
        [StringLength(255, ErrorMessage = "必填少于{0}个字")]
        public string Keywords { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        [Display(Name = "点击量")]
        public int Hits { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        [Display(Name = "发布日期")]
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 状态
        /// <remarks>删除-3 关闭-2 审核/草稿0 发布1</remarks>
        /// </summary>
        [Display(Name = "状态")]
        public StateEnum State { get; set; }
        /// <summary>
        /// 排序 
        /// <remarks>默认0 越大越靠前</remarks>
        /// </summary>
        [Display(Name = "排序")]
        public int Sort { get; set; }
        //[Display(Name = "宝贝图片")]
        //[ForeignKey("Product_ID")]
        //public virtual ICollection<ProductImg> ProductImgs { get; set; }
        //[ForeignKey("Product_ID")]
        //public virtual ICollection<ShopCart> ShopCarts { get; set; }
        //[ForeignKey("Product_ID")]
        //public virtual ICollection<Collect> Collects { get; set; }

        ////宝贝规格
        //public int Color { get; set; }

        ////属性 id 9,11
        //public string AttrDetails { get; set; }


        //public Nullable<DateTime> LastTime { get; set; }
        //public bool Close { get; set; }
        //父栏目编号
        //public virtual Column Parent { get; set; }
        //public virtual ICollection<Column> Children { get; set; }
        //public virtual ICollection<FilterCategory> FilterCategorys { get; set; }
        //public virtual ICollection<FilterAttributes> FilterAttributes { get; set; }

        //public virtual ICollection<Category> Categorys { get; set; }
        //public virtual ICollection<Attr> Attrs { get; set; }
        //public virtual ICollection<Attrdata> Attrdatas { get; set; }
        //public virtual ICollection<Category> Categorys { get; set; }
    }
}
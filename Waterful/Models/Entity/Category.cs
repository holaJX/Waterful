using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mincms.Models.Entity
{
    //栏目
    //自反一对多 一对多Product 多对多Attr
    [Table("Category")]
    public class Category
    {
        public Category()
        {
            //this.State = "0001".ToCharArray();
            //this.State = new char[4] { '0', '0', '0', '1'};
            State = StateEnum.发布;
        }
        //[JsonProperty(PropertyName = "id")]
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [StringLength(50, ErrorMessage = "必须少于{0}个字")]
        public string Name { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "英文名")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字")]
        public string Title { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        [Display(Name = "缩略图")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字符")]
        public string Thumb { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        [Display(Name = "链接")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字符")]
        public string Url { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [Display(Name = "层级")]
        [Required(ErrorMessage = "*")]
        public byte Lvl { get; set; }
        /// <summary>
        /// 排序
        /// <remarks>0为默认排序</remarks>
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "*")]
        public byte Sort { get; set; }
        /// <summary>
        /// 状态
        /// <remarks>删除 关闭 审核 发布/草稿</remarks>
        /// </summary>
        [Display(Name = "状态")]
        public StateEnum State { get; set; }
        //[Display(Name = "引用数")]
        //public int Num { get; set; }
        /// <summary>
        /// 0默认 1文章 2商品 5menu
        /// </summary>
        [Display(Name = "类型")]
        public CatType Type { get; set; }
        
        //[Display(Name = "模块ID")]
        //public byte Mod { get; set; }
        //public string ParentName { get; set; }
        //public Nullable<int> ParentID { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        //public virtual ICollection<Goods> Products { get; set; }
        //public virtual ICollection<Article> Articles { get; set; }
        //public virtual ICollection<Attr> Attrs { get; set; }
        
    }
   
    ////属性 
    ////多对多Category 一对多Attrdata
    //public class Attr
    //{
    //    //序号
    //    [Key]
    //    public int ID { get; set; }
    //    [Display(Name = "名称")]
    //    [StringLength(50, ErrorMessage = "必须少于{0}个字")]
    //    public string Name { get; set; }
    //    //排序
    //    public int Sort { get; set; }
    //    //状态
    //    public int State { get; set; }
    //    public int CategoryID { get; set; }
    //    public virtual Category Category { get; set; }
    //    public virtual ICollection<Attrdata> Attrdatas { get; set; }
    //    public virtual ICollection<Product> Products { get; set; }
    //    //[ForeignKey("CatID")]
    //}

    ////属性值
    ////被Attr一对多
    //public class Attrdata
    //{
    //    [Key]
    //    public int ID { get; set; }
    //    public string Name { get; set; }
    //    public int Sort { get; set; }
    //    public int AttrID { get; set; }
    //    public virtual Attr Attr { get; set; }
    //    public virtual ICollection<Product> Products { get; set; }

    //}

    //中间  栏目_筛选类别_筛选属性
    //属性 int pk
    //栏目编号
    //筛选类别_筛选属性关系编号
    //排序
    /*
    栏目:      手机数码分类  >
                             >  手机配件                手机                     数码影像              数码配件  
                             > 电池 保护套       IPhone 小米 翻盖 直板       便携相机 单反相机            ...

    筛选类别: 网络制式               品牌                 价格
    筛选属性: GSM CDMA 联通 电信    三星 摩托罗拉     1-499 500-999
    */
}
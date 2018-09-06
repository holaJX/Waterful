using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mincms.Models.Entity
{
    //文章 和标签多对多
    [Table("Article")]
    public class Article
    {
        public Article()
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
        [StringLength(255, ErrorMessage = "必须少于{0}个字")]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [DataType(DataType.Html)]
        public string Content { get; set; }
        /// <summary>
        /// 录入者
        /// </summary>
        [Display(Name = "录入者")]
        [StringLength(50, ErrorMessage = "必须少于{0}个字")]
        public string Inputer { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [Display(Name = "作者")]
        [StringLength(50, ErrorMessage = "必须少于{0}个字")]
        public string Author { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        [Display(Name = "来源")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字")]
        public string Source { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        [Display(Name = "缩略图")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字符")]
        public string Thumb { get; set; }
        /// <summary>
        /// 摘要Intro
        /// </summary>
        [Display(Name = "摘要")]
        [StringLength(255, ErrorMessage = "必填少于{0}个字")]
        public string Description { get; set; }
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

        //public virtual ICollection<Category> Categorys { get; set; }
    }
}
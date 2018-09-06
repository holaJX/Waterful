using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mincms.Models.Entity
{
    //横幅 广告 展示图
    [Table("Banner")]
    public class Banner
    {
        public Banner()
        {
            AddTime = DateTime.Now;
            //this.GUID = Guid.NewGuid();
            //this.AddTime = DateTime.Now;
            //this.Parent = new Column();
            //this.Children = new List<Column>();
            //this.Tags = new List<Tag>();
            //this.SortID = 0;
        }
        //序号
        [Key]
        public int Id { get; set; }
        //栏目编号
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public int Sort { get; set; }
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        public StateEnum State { get; set; }
        /// <summary>
        /// 之后考虑xml或数据库来维护
        /// </summary>
        public BannerType Type { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        //父栏目编号
        //public virtual Column Parent { get; set; }
        //public virtual ICollection<Column> Children { get; set; }
        //public virtual ICollection<FilterCategory> FilterCategorys { get; set; }
        //public virtual ICollection<FilterAttributes> FilterAttributes { get; set; }

    }

}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace mincms.Models.Entity
{
    //区域
    [Table("Area")]
    public class Area
    {
        public Area()
        {
            //this.GUID = Guid.NewGuid();
            //this.AddTime = DateTime.Now;
            //this.Parent = new Column();
            //this.Children = new List<Column>();
            //this.Tags = new List<Tag>();
            //this.SortID = 0;
        }
        //[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }
        public Nullable<int> ParentId { get; set; }
        //栏目名称
        //[MaxLength(20)]
        //public int Name { get; set; }
        public string Name { get; set; }

        public virtual Area Parent { get; set; }
        public virtual ICollection<Area> Children { get; set; }
        
        //父栏目编号
        //public virtual Column Parent { get; set; }
        //public virtual ICollection<Column> Children { get; set; }
        //public virtual ICollection<FilterCategory> FilterCategorys { get; set; }
        //public virtual ICollection<FilterAttributes> FilterAttributes { get; set; }
        
    }

}
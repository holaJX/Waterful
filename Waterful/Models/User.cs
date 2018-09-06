using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Models
{
    //[Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [Display(Name = "发布日期")]
        public string LastName1 { get; set; }
    }
    public class User123
    {
      
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string Name1 { get; set; }
    }
}

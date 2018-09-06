using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Wechat.ViewModels
{
    public class ShareGetDetailVM
    {
        public string CreateTime { get; set; }
        public string OrderAmount { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
    }

    public class ShareGetDetailPostVM
    {
        [Range(1, int.MaxValue)]
        public int PageIndex { get; set; }
        [Range(0, 2)]
        public int ShareType { get; set; }
    }

    public class ShareDetailVM
    {
        public bool IsAgent { get; set; }
        public int ShareNum { get; set; }
        public int PayNum { get; set; }
        public decimal Total { get; set; }
        public decimal Amount { get; set; }
    }
}

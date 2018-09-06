using System;
using System.Collections.Generic;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class Captcha : Entity
    {
        public string Phone { get; set; }
        public string Code { get; set; }
        public int Num { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

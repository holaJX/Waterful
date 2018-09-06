using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Wechat.ViewModels
{
    public class PassportLoginVM
    {
        [Required(ErrorMessage = "手机号码错误。")]
        [Phone(ErrorMessage = "手机号码错误。")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "验证码必填。")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "验证码必填。")]
        public string ValidCode { get; set; }
    }
}

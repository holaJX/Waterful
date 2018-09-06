using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class Customer : Entity
    {
        public Customer()
        {
            var dt = DateTime.Now;
            IntroducId = 0;
            Status = 1;
            RegisterType = 0;
            IsPay = false;
            IsAngel = false;
            CreateTime = dt;
            UpdateTime = dt;
        }
        /// <summary>
        /// 微信Id
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string NickName { get; set; }
        public string Mobile { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Ico { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        public string QrImg { get; set; }
        /// <summary>
        /// 推荐人Id
        /// </summary>
        public int IntroducId { get; set; }
        /// <summary>
        /// 状态,0=无效,1=有效
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 注册类型 0 微信创建 1 老数认证生成
        /// </summary>
        public int RegisterType { get; set; }
        /// <summary>
        /// 是否支付过(支付过则为天使用户,支付成功标识为true)
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 是否大使用户
        /// </summary>
        public bool IsAngel { get; set; }

        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}

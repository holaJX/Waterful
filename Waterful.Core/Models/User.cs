using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 管理员表
    /// </summary>
    public class User : Entity
    {
        public User()
        {
            var dt = DateTime.Now;
            Status = 1;
            CreateUser = 0;
            CreateTime = dt;
            UpdateTime = dt;
            LoginTime = dt;
        }

        [MaxLength(30)]
        public string UserName { get; set; }
        [MaxLength(30)]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 状态 -1 删除 0 关闭 1正常
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }
        public int CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 最后一次登录成功时间
        /// </summary>
        public DateTime LoginTime { get; set; }
    }
}

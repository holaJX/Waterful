using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 聊天记录管理(根据产品要求实际功能为用户画像管理,做了对调)
    /// </summary>
    public class Userchat : Entity
    {
        public Userchat()
        {
            var dt = DateTime.Now;
            Status = 1;
            CreateTime = dt;
            UpdateTime = dt;
        }
        public string Name { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// 状态 0 默认 -1 删除
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

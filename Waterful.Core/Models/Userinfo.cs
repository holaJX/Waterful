using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 用户画像管理(根据产品要求实际功能为聊天记录管理,做了对调)
    /// </summary>
    public class Userinfo : Entity
    {
        public Userinfo()
        {
            var dt = DateTime.Now;
            Status = 1;
            CreateTime = dt;
            UpdateTime = dt;
        }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// 状态 1 正常 0 离职 -1 删除
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

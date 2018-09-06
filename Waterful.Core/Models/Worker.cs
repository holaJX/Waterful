using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 服务人员表
    /// </summary>
    public class Worker : Entity
    {
        public Worker()
        {
            var dt = DateTime.Now;
            Status = 1;
            CreateTime = dt;
            UpdateTime = dt;
        }
        /// <summary>
        /// 工号
        /// </summary>
        public string WorkerNo { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 员工头像
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 状态 1 正常 0 离职 -1 删除
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

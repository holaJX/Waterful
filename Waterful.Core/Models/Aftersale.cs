using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Waterful.Core.Models
{
    /// <summary>
    /// 售后服务表
    /// </summary>
    public class Aftersale : Entity
    {
        public Aftersale()
        {
            var dt = DateTime.Now;
            OrderId = 0;
            WorkerId = 0;
            Status = 0;
            ServiceTime = dt;
            CreateTime = dt;
            UpdateTime = dt;
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        public int WorkerId { get; set; }
        /// <summary>
        /// 服务时间
        /// </summary>
        public DateTime ServiceTime { get; set; }


        /// <summary>
        /// 服务状态,0=已派遣(创建服务 后台派遣),1=已服务(用户点击确认服务) 2= 已评价(完成 评价后状态)
        /// </summary>
        public int Status { get; set; }
        public string Remark { get; set; }


        #region 服务状态为1（评论）
        /// <summary>
        /// 评级(1-5)
        /// </summary>
        public int Grade { get; set; }
        /// <summary>
        /// 上门服务是否准时
        /// </summary>
        public bool isOnTime { get; set; }
        /// <summary>
        /// 服装是否整齐
        /// </summary>
        public bool isTidy { get; set; }
        /// <summary>
        /// 完工是否整洁
        /// </summary>
        public bool isClear { get; set; }
        /// <summary>
        /// 改进建议
        /// </summary>
        public string Content { get; set; }
       
        #endregion
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public virtual Worker Worker { get; set; }
        public virtual Order Order { get; set; }
    }
}

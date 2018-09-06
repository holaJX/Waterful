using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Wechat.ViewModels
{
    public class CommentVM
    {
        /// <summary>
        /// 服务id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public int OId { get; set; }
        public string Name { get; set; }
        public string ServiceTime { get; set; }
        /// <summary>
        /// 员工头像
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// Grade星级
        /// </summary>
        public int star { get; set; }
        /// <summary>
        /// 上门服务是否准时
        /// </summary>
        public int isOnTime { get; set; }
        /// <summary>
        /// 服装是否整齐
        /// </summary>
        public int isTidy { get; set; }
        /// <summary>
        /// 完工是否整洁
        /// </summary>
        public int isClear { get; set; }
        /// <summary>
        /// 改进建议
        /// </summary>
        public string Content { get; set; }
    }
}

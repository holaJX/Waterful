using System;
using System.Collections.Generic;
using System.Text;
using Waterful.Core.Enums;

namespace Waterful.Core.DTO
{
    public class ShareDto
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        /// <summary>
        /// 关注人数
        /// </summary>
        public int Count { get; set; }
        public int PayCount { get; set; }
    }
}

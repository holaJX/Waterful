using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GZ.Platform.Core.Options
{
    public class FileOption
    {
        /// <summary>
        /// 前台地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 前台文件绝对路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 秘钥(暂未启用)
        /// </summary>
        public string Key { get; set; }
    }
}

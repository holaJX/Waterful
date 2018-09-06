using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Back.ViewModels
{
    public class AjaxResult<T>
    {
        /// <summary>
        /// 0为失败 1为成功
        /// </summary>
        public int err { get; set; }
        public string msg { get; set; }

        public T data { get; set; }
    }

    public class AjaxResult
    {
        /// <summary>
        /// 0为失败 1为成功
        /// </summary>
        public int err { get; set; }
        public string msg { get; set; }

    }
}

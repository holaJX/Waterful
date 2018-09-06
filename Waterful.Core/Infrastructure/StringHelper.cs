using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Waterful.Core.Infrastructure
{
    public static class StringHelper
    {
        /// <summary>
        /// 返回所有错误信息
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string SubText(string s, int len)
        {
            if (s == null)
                return "";
            if (s.Length > len)
                return s.Substring(0, len) + "...";
            return s;
        }
    }
}

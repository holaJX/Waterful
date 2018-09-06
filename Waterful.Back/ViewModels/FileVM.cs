using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Back.ViewModels
{
    public class FileVM
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public string url { get; set; }
    }
    /// <summary>
    /// UMEditor返回实体
    /// </summary>
    public class UMEditorVM
    {
        public string state { get; set; } = "SUCCESS";
        public string url { get; set; } = "";
        public string originalName { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public string type { get; set; }
    }
}


using System.Net;

namespace mincms.Models.ViewModel
{
    public class JsonModel
    {
        public string desc { get; set; }
        public int code { get; set; } = 0;
        public void Set(HttpStatusCode hsc)
        {
            this.code = hsc.GetHashCode();
            this.desc = hsc.ToString();
        }
        public void Set(int code, string desc)
        {
            this.code = code;
            this.desc = desc;
        }
    }
}
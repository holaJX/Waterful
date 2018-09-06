using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Waterful.Wechat.Extensions
{
    public static class ModelStateExtension
    {
        /// <summary>
        /// 返回所有错误信息
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string ErrorMessages(this ModelStateDictionary modelState, string separator = "")
        {
            return string.Join(separator, modelState.Values.SelectMany(m => m.Errors).Select(m => m.ErrorMessage));
        }
    }
}

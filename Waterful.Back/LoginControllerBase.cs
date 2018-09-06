using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Back
{
    public abstract class LoginControllerBase : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            byte[] result;
            filterContext.HttpContext.Session.TryGetValue("CurrentUserId", out result);
            if (result == null)
            {
                if (filterContext.HttpContext.Request.Method.ToLower()=="get")
                {
                    filterContext.Result = new RedirectResult("/Login/Index?url=" + System.Net.WebUtility.HtmlDecode(filterContext.HttpContext.Request.Path));//  Server.UrlEncode(filterContext.HttpContext.Request.Path);
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
                return;
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}

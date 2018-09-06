using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Waterful.Core;
using Waterful.Back.ViewModels;
using Waterful.Back.App;
using Waterful.Core.Infrastructure;
using X.PagedList;

namespace Waterful.Back.Controllers
{
    public class LoginController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public LoginController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            ViewData["url"] = Request.Query["url"];
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.UserRepository.SingleOrDefault(m => m.UserName == model.UserName);

                //检查用户信息
                //var user = _unitOfWork.UserRepository.CheckUser(model.UserName, model.Password);
                if (user != null)
                {
                    //if (Utilities.VerifyHashedPassword(user.Password, model.Password))
                    //{
                        //记录Session
                        HttpContext.Session.SetString("CurrentUserId", user.Id.ToString());
                        //HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));

                        var url = HttpContext.Request.Query["url"];
                        if (!string.IsNullOrWhiteSpace(url) && Url.IsLocalUrl(url))
                        {
                            //跳转登录页
                            return Redirect(url);
                        }
                        else
                        {
                            //跳转到系统首页
                            return RedirectToAction("Index", "Home");
                        }
                    //}
                }
                ViewBag.ErrorInfo = "用户名或密码错误。";
                return View();
            }
            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return View(model);
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            byte[] result;
            HttpContext.Session.TryGetValue("CurrentUserId", out result);
            if (result != null)
            {
                HttpContext.Session.Remove("CurrentUserId");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
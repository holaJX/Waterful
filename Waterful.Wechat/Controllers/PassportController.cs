using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Waterful.Wechat.ViewModels;
using Waterful.Wechat.Extensions;
using Waterful.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.Exceptions;
using Microsoft.Extensions.Options;
using Waterful.Wechat.WeiXinHandler;
using Senparc.Weixin.MP;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Containers;
using System.Text.RegularExpressions;

namespace Waterful.Wechat.Controllers
{
    [Authorize]
    public class PassportController : Controller
    {
        private readonly ILogger _logger;
        private readonly WeixinConfigSetting _weiXinConfigSetting;
        private readonly UnitOfWork _unitOfWork;

        public PassportController(ILogger<PassportController> logger, IOptions<WeixinConfigSetting> weixinConfigSetting, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _weiXinConfigSetting = weixinConfigSetting.Value;
        }

        #region Login
        [AllowAnonymous]
        public async Task<IActionResult> TestLogin(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                var csm = _unitOfWork.CustomerRepository.FirstOrDefault(m => m.Id > 0);

                await Login(csm.Id, csm.Mobile, csm.OpenId);
            }
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(PassportLoginVM model, string returnUrl)
        {
            AjaxResult dto = new AjaxResult();
            try
            {
                if (!ModelState.IsValid)
                {
                    dto.msg = ModelState.ErrorMessages();
                }
                if (User.Identities.PhoneExist())
                {
                    dto.msg = "无需重复绑定";
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    var end = dt.AddMinutes(-10);
                    var capt = _unitOfWork.CaptchaRepository.Any(m => m.Phone == model.Phone && m.Code == model.ValidCode && m.UpdateTime > end);
                    if (!capt)
                    {
                        dto.msg = "验证码无效。";
                    }
                    else
                    {
                        int cid = User.Identities.DefaultCustomerId();

                        var csm = _unitOfWork.CustomerRepository.SingleOrDefault(m => m.Id == cid);

                        if (string.IsNullOrEmpty(csm.Mobile))
                        {
                            csm.Mobile = model.Phone;
                            _unitOfWork.CustomerRepository.Update(csm);
                        }
                        await Login(csm.Id, csm.Mobile, csm.OpenId);
                        dto.err = 1;
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            dto.msg = returnUrl;
                        }
                        else
                        {
                            dto.msg = Url.Action("Index", "Home");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError("{0}-{1}", nameof(Login), ex);
            }
            return Ok(dto);
        }

        #endregion

        #region Auth2.0
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl">用户尝试进入的需要登录的页面</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Auth(string returnUrl)
        {
            if (Request.Headers["User-Agent"].ToString().ToLower().Contains("micromessenger"))
            {
                var state = "Waterful-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性

                var url =
                    OAuthApi.GetAuthorizeUrl(_weiXinConfigSetting.MP.WeixinAppId,
                    _weiXinConfigSetting.AuthUrl + "/BaseCallback?returnUrl=" + returnUrl.UrlEncode(),
                    state, OAuthScope.snsapi_base);
                return Redirect(url);
            }
            else
            {
                return View("Subscribe");
            }
        }
        /// <summary>
        /// OAuthScope.snsapi_base方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="returnUrl">用户最初尝试进入的页面</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> BaseCallback(string code, string state, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Content("您拒绝了授权！");
            }

            if (string.IsNullOrWhiteSpace(state) || !state.StartsWith("Waterful-", StringComparison.CurrentCultureIgnoreCase))
            {
                return Content("验证失败！请从正规途径进入！");
            }
            try
            {
                //通过，用code换取access_token
                var result = OAuthApi.GetAccessToken(_weiXinConfigSetting.MP.WeixinAppId, _weiXinConfigSetting.MP.WeixinAppSecret, code);
                if (result.errcode != ReturnCode.请求成功)
                {
                    return Content("错误：" + result.errmsg);
                }

                var csm = _unitOfWork.CustomerRepository.SingleOrDefault(m => m.OpenId == result.openid);
                if (csm != null)
                {
                    if (csm.Status == 1)
                    {
                        await Login(csm.Id, csm.Mobile, csm.OpenId);
                    }
                    else
                    {
                        return Content("该账号无效。");
                    }
                }
                else
                {
                    var accessToken = AccessTokenContainer.TryGetAccessToken(_weiXinConfigSetting.MP.WeixinAppId, _weiXinConfigSetting.MP.WeixinAppSecret);
                    //已关注，可以得到详细信息
                    var userInfo = UserApi.Info(accessToken, result.openid);
                    if (userInfo != null && userInfo.subscribe == 1)
                    {
                        csm = new Core.Models.Customer();
                        csm.OpenId = result.openid;
                        csm.CreateTime = DateTime.Now;
                        csm.UpdateTime = csm.CreateTime;
                        csm.Status = 1;

                        csm.NickName = userInfo.nickname;
                        csm.Ico = userInfo.headimgurl;

                        _unitOfWork.CustomerRepository.Insert(csm);
                        await Login(csm.Id, csm.Mobile, csm.OpenId);
                    }
                    else
                    {
                        return View("Subscribe");
                    }
                }
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0}-{1}-{2}-{3}", nameof(Auth), ex, ex.Source, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError(1, "{0}-{1}-{2}-{3}", nameof(Auth), ex.InnerException, ex.InnerException.Source, ex.InnerException.StackTrace);
                }
            }
            return View("Subscribe");
            //return Content("出错啦，请重新访问。");
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        #region SMS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Message(string id)
        {
            AjaxResult ar = new AjaxResult();
            try
            {
                if (!string.IsNullOrEmpty(id) && Regex.IsMatch(id, @"^(\+86)?1[3,4,5,7,8](\d{9})$"))
                {
                    DateTime dt = DateTime.Now;
                    var cap = _unitOfWork.CaptchaRepository.SingleOrDefault(m => m.Phone == id);
                    if (cap == null)
                    {
                        cap = new Core.Models.Captcha();
                        cap.Phone = id;
                        cap.Num = 1;
                        cap.Code = new Random().Next(100000, 999999).ToString();
                        cap.UpdateTime = dt;
                        cap.CreateTime = dt;
                        _unitOfWork.CaptchaRepository.Insert(cap);

                        var send = new MessageService().SendSmsCode(id, cap.Code);
                        if (send != null && send.Result.Success)
                        {
                            ar.err = 1;
                            ar.msg = "发送成功。";
                        }
                        else
                        {
                            ar.msg = "短信服务掉链子啦,请稍后再偿试！";
                        }
                    }
                    else
                    {
                        var dtc = (dt - cap.UpdateTime);
                        if (dtc.TotalMinutes < 1)
                        {
                            ar.msg = "操作过于频繁，请1分钟后再获取。";
                        }
                        else
                        {
                            if (cap.Num > 4 && dtc.Hours < 1)
                            {
                                ar.msg = $"操作过于频繁，请{(60 - dtc.Minutes).ToString()}分钟后再试。";
                            }
                            else
                            {
                                if (dtc.Days >= 1)
                                {
                                    cap.Num = 1;
                                    cap.Code = new Random().Next(100000, 999999).ToString();
                                    cap.UpdateTime = dt;
                                    _unitOfWork.SaveChange();

                                    var send = new MessageService().SendSmsCode(id, cap.Code);
                                    if (send != null && send.Result.Success)
                                    {
                                        ar.err = 1;
                                        ar.msg = "发送成功。";
                                    }
                                    else
                                    {
                                        ar.msg = "短信服务掉链子啦,请稍后再偿试！";
                                    }
                                }
                                else
                                {
                                    if (cap.Num > 9)
                                    {
                                        if (dtc.Hours <= 12)
                                        {
                                            ar.msg = "操作过于频繁，请明天再来。";
                                        }
                                        else if (dtc.Hours < 23)
                                        {
                                            ar.msg = $"操作过于频繁，请{(24 - dtc.Hours).ToString()}小时后再试。";
                                        }
                                        else
                                        {
                                            ar.msg = $"操作过于频繁，请{(60 - dtc.Minutes).ToString()}分钟后再试。";
                                        }
                                    }
                                    else
                                    {
                                        ++cap.Num;
                                        cap.Code = new Random().Next(100000, 999999).ToString();
                                        cap.UpdateTime = dt;
                                        _unitOfWork.SaveChange();

                                        var send = new MessageService().SendSmsCode(id, cap.Code);
                                        if (send != null && send.Result.Success)
                                        {
                                            ar.err = 1;
                                            ar.msg = "发送成功。";
                                        }
                                        else
                                        {
                                            ar.msg = "短信服务掉链子啦,请稍后再偿试！";
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                else
                {
                    ar.msg = "手机号码错误。";
                }
            }
            catch (Exception ex)
            {
                ar.msg = "系统繁忙，请稍后尝试。";
                _logger.LogError("{0}-{1}", nameof(Message), ex);
            }
            return Ok(ar);
        }
        #endregion

        #region Helper
        /// <summary>
        /// 写cookie
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="phone"></param>
        /// <param name="openId"></param>
        private async Task Login(int cid, string phone, string openId)
        {

            var identity = new ClaimsIdentity(
                                    new List<Claim>
                                    {
                                    new Claim(ClaimTypes.NameIdentifier, cid.ToString(), ClaimValueTypes.UInteger32),
                                    new Claim(ClaimTypes.PrimarySid, openId, ClaimValueTypes.String)
                                    }, "Basic");
            if (!string.IsNullOrEmpty(phone))
            {
                identity.AddClaim(new Claim(ClaimTypes.MobilePhone, phone, ClaimValueTypes.String));
            }
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
        }
        #endregion
    }
}
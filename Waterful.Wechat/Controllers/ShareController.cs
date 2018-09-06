using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Waterful.Wechat.WeiXinHandler;
using Waterful.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP;
using System.Net.Http;
using System.IO;
using GZ.Platform.Core.Options;
using Waterful.Wechat.Extensions;
using Microsoft.AspNetCore.Authorization;
using Waterful.Wechat.ViewModels;
using Senparc.Weixin.MP.Containers;
using Microsoft.AspNetCore.Http;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.HttpUtility;

namespace Waterful.Wechat.Controllers
{
    public class ShareController : Controller
    {
        private readonly WeixinConfigSetting _weiXinConfigSetting;
        private readonly ILogger _logger;
        private readonly UnitOfWork _unitOfWork;
        private readonly FileOption _fileOption;

        public ShareController(ILogger<ShareController> logger, IOptions<WeixinConfigSetting> weixinConfigSetting, IOptions<FileOption> fileOption, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _weiXinConfigSetting = weixinConfigSetting.Value;
            _unitOfWork = unitOfWork;
            _fileOption = fileOption.Value;
        }

        #region Index
        public IActionResult Index(int? id)
        {
            if (!id.HasValue)
            {
                if (User.Identity.IsAuthenticated)
                {
                    id = User.Identities.DefaultCustomerId();
                    return RedirectToAction(nameof(Qr), new { id = id.Value });
                }
                else
                {
                    if (Request.Headers["User-Agent"].ToString().ToLower().Contains("micromessenger"))
                    {
                        return RedirectToAction(nameof(PassportController.Auth), "Passport", new { returnUrl = Url.Action(nameof(Index)) });
                    }
                }
                return NotFound();
            }
            else
            {
                return RedirectToAction(nameof(Qr), new { id = id.Value });
            }
        }

        public async Task<IActionResult> Qr(int id)
        {
            try
            {
                if (id > 0)
                {
                    var csm = await _unitOfWork.CustomerRepository.GetQrImgAsync(id);
                    if (csm == null)
                    {
                        return NotFound();
                    }
                    if (string.IsNullOrEmpty(csm.QrImgUrl) && User.Identity.IsAuthenticated)
                    {
                        csm.QrImgUrl = await GetOrCreateQrImg(id.ToString());
                        if (string.IsNullOrEmpty(csm.QrImgUrl))
                        {
                            return Content("系统异常，请稍后再访问。");
                        }
                        else
                        {
                            var cs = _unitOfWork.CustomerRepository.SingleOrDefault(m => m.Id == id);
                            cs.QrImg = csm.QrImgUrl;
                            cs.UpdateTime = DateTime.Now;
                            _unitOfWork.CustomerRepository.Update(cs);
                        }
                    }
                    if (!string.IsNullOrEmpty(csm.QrImgUrl))
                    {
                        ViewData["WxExplorer"] = Request.Headers["User-Agent"].ToString().ToLower().Contains("micromessenger");
                        return View(csm);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0}-{1}-{2}-{3}", nameof(Qr), ex, ex.Source, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError(1, "{0}-{1}-{2}-{3}", nameof(Qr), ex.InnerException, ex.InnerException.Source, ex.InnerException.StackTrace);
                }
            }
            return NotFound();
        }

        #endregion

        #region Detail
        [Authorize]
        public async Task<IActionResult> Detail(int id = 0)
        {
            if (id < 0 || id > 2)
            {
                return NotFound();
            }
            int cid = User.Identities.DefaultCustomerId();
            ShareDetailVM vm = new ShareDetailVM();
            vm.ShareNum = await _unitOfWork.CustomerRepository.CountAsync(m => m.IntroducId == cid);
            vm.PayNum = await _unitOfWork.CustomerRepository.CountAsync(m => m.IntroducId == cid && m.IsPay);

            if (await _unitOfWork.CustomerRepository.ExistAsync(m => m.Id == cid && m.IsAngel))
            {
                vm.IsAgent = true;
                vm.Total = await _unitOfWork.CommissionRepository.SumAsync(m => m.Amount, m => m.CustomerId == cid);
                vm.Amount = await _unitOfWork.CommissionRepository.SumAsync(m => m.Amount, m => m.CustomerId == cid && m.Status == 0);
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetDetail(ShareGetDetailPostVM model)
        {
            if (ModelState.IsValid)
            {
                int cid = User.Identities.DefaultCustomerId();
                if (model.ShareType == 0)//关注
                {
                    var list = await _unitOfWork.CustomerRepository.GetChildrenNickNameAsync(model.PageIndex, 20, m => m.IntroducId == cid);
                    return Ok(list);
                }
                else if (model.ShareType == 1)//已购买
                {
                    var list = await _unitOfWork.CustomerRepository.GetChildrenNickNameAsync(model.PageIndex, 10, m => m.IntroducId == cid && m.IsPay);
                    return Ok(list);
                }
                else
                {//提成

                    var list = await _unitOfWork.CommissionRepository.PagerAsync(model.PageIndex, 10, m => m.CustomerId == cid);

                    var nlist = list.Select(m => new ShareGetDetailVM
                    {
                        Amount = m.Amount.ToString("C"),
                        CreateTime = m.CreateTime.ToString("yyyy年MM月dd日 HH:mm"),
                        OrderAmount = m.OrderAmount.ToString("C"),
                        Rate = m.Rate.ToString("P"),
                        Status = m.Status == 0 ? "未结算" : "已结算"
                    });

                    return Ok(nlist);
                }
            }

            return Ok();
        }

        #endregion

        #region Share
        [AllowAnonymous]
        public IActionResult JsSdk(string url)
        {
            var dto = new AjaxResult<JsSdkUiPackage>();
            try
            {
                if (!string.IsNullOrEmpty(url)
                    && url.StartsWith("http://wechat.icj20.com/", StringComparison.CurrentCultureIgnoreCase)
                    && Request.Headers["User-Agent"].ToString().ToLower().Contains("micromessenger")
                    )
                {

                    //获取时间戳
                    var timestamp = JSSDKHelper.GetTimestamp();
                    //获取随机码
                    string nonceStr = Guid.NewGuid().ToString();
                    string ticket = JsApiTicketContainer.TryGetJsApiTicket(_weiXinConfigSetting.MP.WeixinAppId, _weiXinConfigSetting.MP.WeixinAppSecret);
                    //获取签名
                    string signature = JSSDKHelper.GetSignature(ticket, nonceStr, timestamp, url);
                    dto.err = 1;
                    dto.data = new JsSdkUiPackage(_weiXinConfigSetting.MP.WeixinAppId, timestamp, nonceStr, signature);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("{0}-{1}-{2}-{3}", nameof(JsSdk), ex, ex.Source, ex.StackTrace);
            }
            return Ok(dto);
        }
        #endregion

        #region Helper
        private async Task<string> GetOrCreateQrImg(string cid)
        {
            // 永久二维码调用此方法
            var accessToken = await AccessTokenContainer.TryGetAccessTokenAsync(_weiXinConfigSetting.MP.WeixinAppId, _weiXinConfigSetting.MP.WeixinAppSecret);

            var qrResult = await QrCodeApi.CreateAsync(accessToken, 0, 0, QrCode_ActionName.QR_LIMIT_STR_SCENE, cid);
            var qrCodeUrl = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);


            string qrpath = $"{_fileOption.Path}/qrshare/{cid}.png";
            using (HttpClient httpClient = new HttpClient())
            {
                var strm = await httpClient.GetStreamAsync(qrCodeUrl);
                using (FileStream fs = System.IO.File.Create(qrpath))
                {
                    await strm.CopyToAsync(fs);
                    await fs.FlushAsync();

                    return $"{_fileOption.Url}/qrshare/{cid}.png";
                }
            }
        }
        #endregion
    }
}
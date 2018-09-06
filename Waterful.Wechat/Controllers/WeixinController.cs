using System;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Waterful.Wechat.WeiXinHandler;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using Senparc.Weixin.MP.CoreMvcExtension;
using Waterful.Core;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities;

namespace Waterful.Wechat.Controllers
{
    public class WeixinController : Controller
    {
        private readonly WeixinConfigSetting _weiXinConfigSetting;
        private readonly ILogger _logger;
        private readonly UnitOfWork _unitOfWork;

        public WeixinController(ILogger<WeixinController> logger, IOptions<WeixinConfigSetting> weixinConfigSetting, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _weiXinConfigSetting = weixinConfigSetting.Value;
            _unitOfWork = unitOfWork;
        }

        #region Index
        [HttpGet]
        public IActionResult Index(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, _weiXinConfigSetting.MP.Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content(string.Empty);
                //return Content("failed:" + postModel.Signature + ","
                //    + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, _weiXinConfigSetting.MP.Token) + "。" +
                //    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        [HttpPost]
        public IActionResult Index(PostModel postModel)
        {
            try
            {
                if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, _weiXinConfigSetting.MP.Token))
                {
                    return Content("参数错误！");
                }

                postModel.AppId = _weiXinConfigSetting.MP.WeixinAppId;
                postModel.EncodingAESKey = _weiXinConfigSetting.MP.EncodingAESKey;
                postModel.Token = _weiXinConfigSetting.MP.Token;
                //重要
                var inputStream = new MemoryStream();
                Request.Body.CopyTo(inputStream);

                var messageHandler = new CustomMessageHandler(_logger, _weiXinConfigSetting, _unitOfWork, inputStream, postModel);
                messageHandler.OmitRepeatedMessage = true;//启用消息去重功能
                messageHandler.Execute();//执行微信处理过程（第二步）

                if (messageHandler.ResponseMessage.MsgType == ResponseMsgType.Text)
                {
                    var rpsmsg = (ResponseMessageText)messageHandler.ResponseMessage;
                    if (string.IsNullOrEmpty(rpsmsg.Content))
                    {
                        return Content(string.Empty);
                    }
                }
                return Content(messageHandler.ResponseDocument.ToString(), "application/xml");//返回（第三步）

            }
            catch (Exception ex)
            {
                _logger.LogError("{0}-{1}-{2}-{3}", nameof(Index), ex, ex.Source, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError(1, "{0}-{1}-{2}-{3}", nameof(Index), ex.InnerException, ex.InnerException.Source, ex.InnerException.StackTrace);
                }
            }

            return Content("系统异常,请稍后尝试。");
        }

        #endregion        

        //public IActionResult Token(string id, string key)
        //{
        //    try
        //    {
        //        if (string.Compare(id, "Token", true) == 0 && string.Compare(key, "anech", true) == 0)
        //        {
        //            var accessToken = AccessTokenContainer.TryGetAccessToken(_weiXinConfigSetting.MP.WeixinAppId, _weiXinConfigSetting.MP.WeixinAppSecret);
        //            return Content(accessToken);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(nameof(Token), ex.Message);
        //    }
        //    return Content(id + key);
        //}
    }

}

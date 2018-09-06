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
                return Content(echostr); //��������ַ������ʾ��֤ͨ��
            }
            else
            {
                return Content(string.Empty);
                //return Content("failed:" + postModel.Signature + ","
                //    + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, _weiXinConfigSetting.MP.Token) + "��" +
                //    "�������������п�����仰��˵���˵�ַ���Ա���Ϊ΢�Ź����˺ź�̨��Url����ע�Ᵽ��Tokenһ�¡�");
            }
        }

        [HttpPost]
        public IActionResult Index(PostModel postModel)
        {
            try
            {
                if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, _weiXinConfigSetting.MP.Token))
                {
                    return Content("��������");
                }

                postModel.AppId = _weiXinConfigSetting.MP.WeixinAppId;
                postModel.EncodingAESKey = _weiXinConfigSetting.MP.EncodingAESKey;
                postModel.Token = _weiXinConfigSetting.MP.Token;
                //��Ҫ
                var inputStream = new MemoryStream();
                Request.Body.CopyTo(inputStream);

                var messageHandler = new CustomMessageHandler(_logger, _weiXinConfigSetting, _unitOfWork, inputStream, postModel);
                messageHandler.OmitRepeatedMessage = true;//������Ϣȥ�ع���
                messageHandler.Execute();//ִ��΢�Ŵ�����̣��ڶ�����

                if (messageHandler.ResponseMessage.MsgType == ResponseMsgType.Text)
                {
                    var rpsmsg = (ResponseMessageText)messageHandler.ResponseMessage;
                    if (string.IsNullOrEmpty(rpsmsg.Content))
                    {
                        return Content(string.Empty);
                    }
                }
                return Content(messageHandler.ResponseDocument.ToString(), "application/xml");//���أ���������

            }
            catch (Exception ex)
            {
                _logger.LogError("{0}-{1}-{2}-{3}", nameof(Index), ex, ex.Source, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError(1, "{0}-{1}-{2}-{3}", nameof(Index), ex.InnerException, ex.InnerException.Source, ex.InnerException.StackTrace);
                }
            }

            return Content("ϵͳ�쳣,���Ժ��ԡ�");
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

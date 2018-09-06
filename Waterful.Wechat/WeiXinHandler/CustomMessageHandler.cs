using System.IO;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.Context;
using Microsoft.Extensions.Logging;
using Waterful.Core;
using Microsoft.Extensions.Options;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin;
using Senparc.Weixin.MP.Containers;
using Waterful.Core.Models;
using System;
using Senparc.Weixin.MP.Helpers;
using Waterful.Core.Enums;

namespace Waterful.Wechat.WeiXinHandler
{
    public class CustomMessageHandler : MessageHandler<MessageContext<IRequestMessageBase, IResponseMessageBase>>
    {
        private readonly ILogger _logger;
        private readonly WeixinConfigSetting _weiXinConfigSetting;
        private readonly UnitOfWork _unitOfWork;
        public CustomMessageHandler(ILogger logger, WeixinConfigSetting weixinConfigSetting, UnitOfWork unitOfWork, Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _weiXinConfigSetting = weixinConfigSetting;
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            //WeixinContext.ExpireMinutes = 3;

            //if (!string.IsNullOrEmpty(postModel.AppId))
            //{
            //    appId = postModel.AppId;//通过第三方开放平台发送过来的请求
            //}

            ////在指定条件下，不使用消息去重
            //base.OmitRepeatedMessageFunc = requestMessage =>
            //{
            //    var textRequestMessage = requestMessage as RequestMessageText;
            //    if (textRequestMessage != null && textRequestMessage.Content == "容错")
            //    {
            //        return false;
            //    }
            //    return true;
            //};
        }

        public CustomMessageHandler(RequestMessageBase requestMessage)
            : base(requestMessage)
        {
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Empty;
            return responseMessage;
        }


        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            string str = requestMessage.EventKey;
            str = !string.IsNullOrEmpty(str) && str.StartsWith("qrscene_", StringComparison.CurrentCultureIgnoreCase) ? str.Substring(8) : string.Empty;//qrscene_为前缀

            string msg = string.Empty;
            if (CustomRegist(requestMessage.FromUserName, str))
            {
                msg = "亲，终于来啦，欢迎光临。";
            }
            else
            {
                msg = "欢迎回来。";
            }
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = msg;
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            string msg = string.Empty;
            if (CustomRegist(requestMessage.FromUserName, requestMessage.EventKey))
            {
                msg = "亲，终于来啦，欢迎光临。";
            }
            else
            {
                msg = "欢迎回来。";
            }
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = msg;
            return responseMessage;
        }

        #region Helper
        private bool CustomRegist(string openid, string eventKey)
        {
            if (!_unitOfWork.CustomerRepository.Any(m => m.OpenId == openid))
            {
                try
                {
                    var accessToken = AccessTokenContainer.TryGetAccessToken(_weiXinConfigSetting.MP.WeixinAppId, _weiXinConfigSetting.MP.WeixinAppSecret);
                    var userInfo = UserApi.Info(accessToken, openid);

                    int pid = 0;
                    int.TryParse(eventKey, out pid);

                    var dt = DateTime.Now;
                    var csm = new Customer();
                    csm.CreateTime = dt;
                    csm.UpdateTime = dt;
                    csm.OpenId = openid;
                    csm.Status = 1;

                    if (userInfo != null)
                    {
                        csm.NickName = userInfo.nickname;
                        csm.Ico = userInfo.headimgurl;
                    }
                    csm.IntroducId = pid;
                    var res = _unitOfWork.CustomerRepository.Insert(csm);
                    if (pid > 0)
                    {
                        SendCoupon(CouponTypeEnum.Used, res.Id, "扫码送代金券", dt);
                    }
                    else
                    {
                        SendCoupon(CouponTypeEnum.FreeInstall, res.Id, "关注立即派送免安装券", dt);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("{0}-{1}-{2}-{3}", nameof(CustomRegist), ex, ex.Source, ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        _logger.LogError(1, "{0}-{1}-{2}-{3}", nameof(CustomRegist), ex.InnerException, ex.InnerException.Source, ex.InnerException.StackTrace);
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 派券（暂时没有金额限制）
        /// </summary>
        /// <param name="couponType"></param>
        /// <param name="customerId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private void SendCoupon(CouponTypeEnum couponType, int customerId, string remark, DateTime dt)
        {
            var coupon = _unitOfWork.CouponRepository.FirstOrDefault(e => e.CouponType == CouponEnum.OnLine && e.Type == couponType && e.Status == 1 && e.CustomerId == 0 && e.ExpiryDate >= dt);
            if (coupon != null)
            {
                coupon.CustomerId = customerId;
                coupon.UpdateTime = dt;
                coupon.Remark += remark;
                _unitOfWork.CouponRepository.Update(coupon);
            }
        }
        #endregion 

    }
}
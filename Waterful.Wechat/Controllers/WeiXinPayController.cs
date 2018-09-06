using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Waterful.Wechat.WeiXinHandler;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterful.Core;
using Waterful.Wechat.Extensions;
using Senparc.Weixin.MP.TenPayLibV3;
using Senparc.Weixin.MP;
using Waterful.Wechat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Waterful.Core.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using Waterful.Core.Enums;

namespace Waterful.Wechat.Controllers
{
    [Authorize]
    public class WeiXinPayController : Controller
    {
        private readonly WeixinConfigSetting _weiXinConfigSetting;
        private readonly ILogger _logger;
        private readonly UnitOfWork _unitOfWork;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly static ConcurrentDictionary<string, string> _concurrentDictionary = new ConcurrentDictionary<string, string>();
        public WeiXinPayController(ILogger<PassportController> logger, IOptions<WeixinConfigSetting> weixinConfigSetting, UnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _weiXinConfigSetting = weixinConfigSetting.Value;
            _httpContextAccessor = httpContextAccessor;
        }


        #region Pay
        public IActionResult Pay(int id, string rqt)
        {
            try
            {
                var cid = User.Identities.DefaultCustomerId();
                var od = _unitOfWork.OrderRepository.SingleOrDefault(m => m.Id == id && m.CustomerId == cid && m.Status == 0);
                if (od == null)
                {
                    return Content("订单不存在！");
                }

                var openId = User.Identities.DefaultCustomerOpenId();
                string sp_billno = od.OrderNo;

                var timeStamp = TenPayV3Util.GetTimestamp();
                var nonceStr = TenPayV3Util.GetNoncestr();

                var body = "万得水";
                var price = 1; //(int)(od.Amount * 100);
                var xmlDataInfo = new TenPayV3UnifiedorderRequestData(_weiXinConfigSetting.MP.WeixinAppId, _weiXinConfigSetting.Pay.MchId, body, sp_billno, price, Request.HttpContext.Connection.RemoteIpAddress.ToString(), _weiXinConfigSetting.PayNotifyUrl, TenPayV3Type.JSAPI, openId, _weiXinConfigSetting.Pay.Key, nonceStr);

                var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口                                                               

                WeixinPayPayVM vm = new WeixinPayPayVM();
                vm.Amount = od.Amount;
                vm.OrderId = od.Id;
                vm.OrderNo = od.OrderNo;

                vm.AppId = _weiXinConfigSetting.MP.WeixinAppId;
                vm.TimeStamp = timeStamp;
                vm.Package = string.Format("prepay_id={0}", result.prepay_id);
                vm.NonceStr = nonceStr;
                vm.PaySign = TenPayV3.GetJsPaySign(_weiXinConfigSetting.MP.WeixinAppId, timeStamp, nonceStr, vm.Package, _weiXinConfigSetting.Pay.Key);

                if (string.Compare(rqt, "ajax", true) == 0)
                {
                    return PartialView(vm);
                }
                else
                {
                    return View(vm);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0}-{1}-{2}-{3}", nameof(Pay), ex, ex.Source, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError(1, "{0}-{1}-{2}-{3}", nameof(Pay), ex.InnerException, ex.InnerException.Source, ex.InnerException.StackTrace);
                }
                //return Content(msg);
            }
            return Content("系统繁忙，请稍后再试！");
        }

        #endregion

        #region Notify
        [AllowAnonymous]
        public IActionResult Notify()
        {
            try
            {
                WeiXinHandler.ResponseHandler resHandler = new WeiXinHandler.ResponseHandler(_httpContextAccessor.HttpContext);

                string return_code = resHandler.GetParameter("return_code");
                string return_msg = resHandler.GetParameter("return_msg");
                resHandler.SetKey(_weiXinConfigSetting.Pay.Key);
                string result_code = resHandler.GetParameter("result_code");
                string oNo = resHandler.GetParameter("out_trade_no");

                string msg = resHandler.ParseXML();
                DateTime dt = DateTime.Now;
                //验证请求是否从微信发过来（安全）
                if (resHandler.IsTenpaySign() &&
                    string.Compare(return_code, "SUCCESS", true) == 0
                    && string.Compare(result_code, "SUCCESS", true) == 0
                    && _concurrentDictionary.TryAdd(oNo, dt.ToString("MMddHHmmss")))
                {
                    decimal aMt = 0;
                    if (decimal.TryParse(resHandler.GetParameter("total_fee"), out aMt) && aMt > 0)
                    {
                        var od = _unitOfWork.OrderRepository.SingleOrDefault(m => m.OrderNo == oNo);
                        if (od?.Status == 0) //&& od.Amount * 100 == aMt)
                        {
                            od.Status = 1;
                            od.PayTime = dt;
                            od.PayNotify = msg;
                            od.UpdateTime = dt;

                            var ag = _unitOfWork.CustomerRepository.CustomerPayInfo(od.CustomerId);
                            if (!ag.IsPay)//天使
                            {
                                var csm = _unitOfWork.CustomerRepository.SingleOrDefault(m => m.Id == od.CustomerId);
                                csm.IsPay = true;
                                csm.UpdateTime = dt;
                                if (ag.IntroducId > 0)
                                {
                                    if (ag.IsAngel)//大使
                                    {
                                        var coms = new Commission();
                                        coms.OrderId = od.Id;
                                        coms.CustomerId = ag.IntroducId;
                                        coms.OrderAmount = od.Amount;
                                        coms.Rate = 0.1M;
                                        coms.Amount = coms.OrderAmount * coms.Rate;
                                        coms.CreateTime = dt;
                                        _unitOfWork.CommissionRepository.Insert(coms, false);
                                    }

                                    var coupon = _unitOfWork.CouponRepository.FirstOrDefault(e => e.CouponType == CouponEnum.OnLine && e.Type == CouponTypeEnum.Used && e.Status == 1 && e.CustomerId == 0 && e.ExpiryDate >= dt);
                                    if (coupon != null)
                                    {
                                        coupon.CustomerId = ag.IntroducId;
                                        coupon.UpdateTime = dt;
                                        coupon.Remark += "老客户转介下单成功获赠代金卷。";
                                    }
                                }
                            }

                            if (od.ParentId > 0)
                            {
                                var pod = _unitOfWork.OrderRepository.SingleOrDefault(m => m.Id == od.ParentId);
                                //处理租用
                                if (pod.DepositAmount > 0)
                                {
                                    pod.NextPayTime = pod.NextPayTime.Value.AddMonths(od.Month);
                                }
                                pod.ServiceNumber += od.ServiceNumber;
                                pod.UpdateTime = dt;
                            }
                            else
                            {
                                if (od.Month > 0)
                                {
                                    od.NextPayTime = dt.AddMonths(od.Month);
                                }
                            }
                            _unitOfWork.SaveChange();
                            _concurrentDictionary.TryRemove(oNo, out oNo);
                            string xml = string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>", return_code, return_msg);
                            return Content(xml, "text/xml");
                        }
                        else
                        {
                            _logger.LogError(1, "{0}-订单：{1}金额异常，报文：{2}", nameof(Notify), oNo, msg);
                        }
                    }
                    else
                    {
                        _logger.LogError(2, "{0}-订单：{1}金额异常，报文：{2}", nameof(Notify), oNo, msg);
                    }
                }
                else
                {
                    _logger.LogError(3, "{0}-订单：{1}安全验证未通过，报文：{2}", nameof(Notify), oNo, msg);
                }
            }
            catch (Exception ex)
            {
                string dtr = DateTime.Now.AddMinutes(-3).ToString("MMddHHmmss");
                var keys = _concurrentDictionary.Where(m => m.Value.CompareTo(dtr) < 0).Select(m => m.Key);
                foreach (var item in keys)
                {
                    _concurrentDictionary.TryRemove(item, out dtr);
                }

                _logger.LogError("{0}-{1}-{2}-{3}", nameof(Notify), ex, ex.Source, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError(1, "{0}-{1}-{2}-{3}", nameof(Notify), ex.InnerException, ex.InnerException.Source, ex.InnerException.StackTrace);
                }
            }
            return Ok();
        }

        #endregion
    }
}
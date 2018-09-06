using Senparc.Weixin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Wechat.WeiXinHandler
{
    public class WeixinConfigSetting
    {
        /// <summary>
        /// 授权回调地址
        /// </summary>
        public string AuthUrl { get; set; }
        /// <summary>
        /// 支付回调地址
        /// </summary>
        public string PayNotifyUrl { get; set; }
        public SenparcWeixinSetting MP { get; set; }
        public WeixingPaySettiong Pay { get; set; }
    }

    public class WeixingPaySettiong
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public string MchId { get; set; }
        /// <summary>
        /// 商户支付密钥Key。登录微信商户后台，进入栏目【账户设置】【密码安全】【API 安全】【API 密钥】
        /// </summary>
        public string Key { get; set; }
    }

}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Waterful.Core
{
    //public class MessageService
    //{
    //    public void SendSms(string phone, string code)
    //    {
    //        ITopClient client = new DefaultTopClient("https://eco.taobao.com/router/rest", "24420321", "5e0f2874876a4e452fd69944de1f0a3a");
    //        AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
    //        req.Extend = "123456";
    //        req.SmsType = "normal";
    //        req.SmsFreeSignName = "万得水";
    //        req.SmsParam = "{code:'" + code + "'}";
    //        req.RecNum = "13000000000";
    //        req.SmsTemplateCode = "SMS_585014";
    //        AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
    //        Console.WriteLine(rsp.Body);
    //    }
    //}


    public class MessageService
    {
        private string url = "https://eco.taobao.com/router/rest";
        private string appKey = "24420321";
        private string appSecret = "5e0f2874876a4e452fd69944de1f0a3a";
        /// <summary>
        /// 验证短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        public SmsResponseALi SendSmsCode(string phone, string code)
        {
            var parms = new Dictionary<string, string>();
            parms.Add(Constants.REC_NUM, phone);
            parms.Add(Constants.SMS_FREE_SIGN_NAME, "万得水");
            parms.Add(Constants.SMS_PARAM, "{code:'" + code + "'}");
            parms.Add(Constants.SMS_TEMPLATE_CODE, "SMS_74770002");
            //parms.Add(Constants.SMS_TEMPLATE_CODE, "SMS_71795248");

            var req = SmsHelper.SendSms(url, appKey, appSecret, DateTime.Now, parms);
            return req.Alibaba_Aliqin_Fc_Sms_Num_Send_Response;
        }
        /// <summary>
        /// 服务短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        public void SendSmsMsg(string phone, string code)
        {
            var parms = new Dictionary<string, string>();
            parms.Add(Constants.REC_NUM, phone);
            parms.Add(Constants.SMS_FREE_SIGN_NAME, "万得水");
            parms.Add(Constants.SMS_PARAM, "{code:'" + code + "'}");
            parms.Add(Constants.SMS_TEMPLATE_CODE, "SMS_71820327");

            var req = SmsHelper.SendSms(url, appKey, appSecret, DateTime.Now, parms);
        }
    }

    public class SmsHelper
    {
        public static string Post(string url, string data, Encoding encoding)
        {
            try
            {
                HttpWebRequest req = WebRequest.CreateHttp(new Uri(url));
                req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                req.Method = "POST";
                req.Accept = "text/xml,text/javascript";
                req.ContinueTimeout = 60000;

                byte[] postData = encoding.GetBytes(data);
                Stream reqStream = req.GetRequestStreamAsync().Result;
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Dispose();

                var rsp = (HttpWebResponse)req.GetResponseAsync().Result;
                var result = GetResponseAsString(rsp, encoding);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static T Post<T>(string url, string data, Encoding encoding)
        {
            try
            {
                var result = Post(url, data, encoding);
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return null;
            }

            StringBuilder query = new StringBuilder();
            bool hasParam = false;

            foreach (KeyValuePair<string, string> kv in parameters)
            {
                string name = kv.Key;
                string value = kv.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        query.Append("&");
                    }

                    query.Append(name);
                    query.Append("=");
                    query.Append(WebUtility.UrlEncode(value));
                    hasParam = true;
                }
            }

            return query.ToString();
        }

        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Dispose();
                if (stream != null) stream.Dispose();
                if (rsp != null) rsp.Dispose();
            }
        }

        public static string GetAlidayuSign(IDictionary<string, string> parameters, string secret, string signMethod)
        {
            //把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);

            //把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            if (Constants.SIGN_METHOD_MD5.Equals(signMethod))
            {
                query.Append(secret);
            }
            foreach (KeyValuePair<string, string> kv in sortedParams)
            {
                if (!string.IsNullOrEmpty(kv.Key) && !string.IsNullOrEmpty(kv.Value))
                {
                    query.Append(kv.Key).Append(kv.Value);
                }
            }

            //使用MD5/HMAC加密
            if (Constants.SIGN_METHOD_HMAC.Equals(signMethod))
            {
                return Hmac(query.ToString(), secret);
            }
            else
            {
                query.Append(secret);
                return Md5(query.ToString());
            }
        }

        public static string Hmac(string value, string key)
        {
            byte[] bytes;
            using (var hmac = new HMACMD5(Encoding.UTF8.GetBytes(key)))
            {
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            StringBuilder result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        public static string Md5(string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        public static SmsResultAli SendSms(string url, string appKey, string appSecret, DateTime timestamp, Dictionary<string, string> parsms)
        {
            var txtParams = new SortedDictionary<string, string>();
            txtParams.Add(Constants.METHOD, "alibaba.aliqin.fc.sms.num.send");
            txtParams.Add(Constants.VERSION, "2.0");
            txtParams.Add(Constants.SIGN_METHOD, Constants.SIGN_METHOD_HMAC);
            txtParams.Add(Constants.APP_KEY, appKey);
            txtParams.Add(Constants.FORMAT, "json");
            txtParams.Add(Constants.TIMESTAMP, timestamp.ToString(Constants.DATE_TIME_FORMAT));
            txtParams.Add(Constants.SMS_TYPE, "normal");
            foreach (var item in parsms)
            {
                txtParams.Add(item.Key, item.Value);
            }

            txtParams.Add(Constants.SIGN, GetAlidayuSign(txtParams, appSecret, Constants.SIGN_METHOD_HMAC));
            var result = Post<SmsResultAli>(url, BuildQuery(txtParams), Encoding.UTF8);

            return result;
        }

    }
    public sealed class Constants
    {
        public const string CHARSET_UTF8 = "utf-8";

        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATE_TIME_MS_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        public const string SIGN_METHOD_MD5 = "md5";
        public const string SIGN_METHOD_HMAC = "hmac";

        public const string LOG_SPLIT = "^_^";
        public const string LOG_FILE_NAME = "topsdk.log";

        public const string ACCEPT_ENCODING = "Accept-Encoding";
        public const string CONTENT_ENCODING = "Content-Encoding";
        public const string CONTENT_ENCODING_GZIP = "gzip";

        public const string ERROR_RESPONSE = "error_response";
        public const string ERROR_CODE = "code";
        public const string ERROR_MSG = "msg";

        public const string SDK_VERSION = "top-sdk-net-20160607";
        public const string SDK_VERSION_CLUSTER = "top-sdk-net-cluster-20160607";

        public const string APP_KEY = "app_key";
        public const string FORMAT = "format";
        public const string METHOD = "method";
        public const string TIMESTAMP = "timestamp";
        public const string VERSION = "v";
        public const string SIGN = "sign";
        public const string SIGN_METHOD = "sign_method";
        public const string PARTNER_ID = "partner_id";
        public const string SESSION = "session";
        public const string FORMAT_XML = "xml";
        public const string FORMAT_JSON = "json";
        public const string SIMPLIFY = "simplify";
        public const string TARGET_APP_KEY = "target_app_key";

        public const string QM_ROOT_TAG_REQ = "request";
        public const string QM_ROOT_TAG_RSP = "response";
        public const string QM_CUSTOMER_ID = "customerId";
        public const string QM_CONTENT_TYPE = "text/xml;charset=utf-8";

        public const string MIME_TYPE_DEFAULT = "application/octet-stream";
        public const int READ_BUFFER_SIZE = 1024 * 4;

        public const string SMS_TYPE = "sms_type";
        public const string REC_NUM = "rec_num";
        public const string SMS_FREE_SIGN_NAME = "sms_free_sign_name";
        public const string SMS_PARAM = "sms_param";
        public const string SMS_TEMPLATE_CODE = "sms_template_code";
    }
    public class SmsResultAli
    {
        public SmsResponseALi Alibaba_Aliqin_Fc_Sms_Num_Send_Response { get; set; }
    }

    public class SmsResponseALi
    {
        public string Request_Id { get; set; }
        public SmsResponseResultAli Result { get; set; }
    }

    public class SmsResponseResultAli
    {
        public string Err_Code { get; set; }

        public string Model { get; set; }

        public bool Success { get; set; }
    }
}


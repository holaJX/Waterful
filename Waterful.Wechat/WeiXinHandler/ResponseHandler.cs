using Microsoft.AspNetCore.Http;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Waterful.Wechat.WeiXinHandler
{
    /// <summary>
    /// 因组件不支持.net core 因此重写该组件，方法待优化
    /// </summary>
    public class ResponseHandler
    {
        /// <summary>
        /// 密钥 
        /// </summary>
        private string Key;

        /// <summary>
        /// appkey
        /// </summary>
        //private string Appkey;

        /// <summary>
        /// 应答的参数
        /// </summary>
        protected Hashtable Parameters;

        /// <summary>
        /// debug信息
        /// </summary>
        //private string DebugInfo;
        /// <summary>
        /// 原始内容
        /// </summary>
        //protected string Content;


        //protected HttpContext HttpContext;

        /// <summary>
        /// 初始化函数
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// 获取页面提交的get和post参数
        /// </summary>
        /// <param name="httpContext"></param>
        public ResponseHandler(HttpContext httpContext)
        {

            Parameters = new Hashtable();

            //this.HttpContext = httpContext ?? new DefaultHttpContext();
            //IFormCollection collection;
            ////post data
            //if (this.HttpContext.Request.Method.ToUpper() == "POST" && this.HttpContext.Request.HasFormContentType)
            //{
            //    collection = this.HttpContext.Request.Form;
            //    foreach (var k in collection)
            //    {
            //        this.SetParameter(k.Key, k.Value[0]);
            //    }
            //}
            ////query string
            //var coll = this.HttpContext.Request.Query;
            //foreach (var k in coll)
            //{
            //    this.SetParameter(k.Key, k.Value[0]);
            //}
            if (httpContext.Request.Body.CanRead)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(httpContext.Request.Body);
                XmlNode root = xmlDoc.SelectSingleNode("xml");
                XmlNodeList xnl = root.ChildNodes;
                foreach (XmlNode xnf in xnl)
                {
                    this.SetParameter(xnf.Name, xnf.InnerText);
                }
            }
        }


        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        { return Key; }

        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="key"></param>
        public void SetKey(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string GetParameter(string parameter)
        {
            string s = (string)Parameters[parameter];
            return (null == s) ? "" : s;
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        public void SetParameter(string parameter, string parameterValue)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                if (Parameters.Contains(parameter))
                {
                    Parameters.Remove(parameter);
                }

                Parameters.Add(parameter, parameterValue);
            }
        }

        /// <summary>
        /// 是否财付通签名,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。return boolean
        /// </summary>
        /// <returns></returns>
        public virtual Boolean IsTenpaySign()
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(Parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];
                if (!string.IsNullOrEmpty(v)
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + this.GetKey());
            string sign = MD5UtilHelper.GetMD5(sb.ToString(), GetCharset());
            //this.SetDebugInfo(sb.ToString() + " &sign=" + sign);
            //debug信息
            return string.Compare(GetParameter("sign"), sign, true) == 0;
        }

        /// <summary>
        /// 获取debug信息
        /// </summary>
        /// <returns></returns>
        //public string GetDebugInfo()
        //{ return DebugInfo; }

        /// <summary>
        /// 设置debug信息
        /// </summary>
        /// <param name="debugInfo"></param>
        //protected void SetDebugInfo(String debugInfo)
        //{ this.DebugInfo = debugInfo; }

        protected virtual string GetCharset()
        {
            return Encoding.UTF8.WebName;
        }

        /// <summary>
        /// 输出XML
        /// </summary>
        /// <returns></returns>
        public string ParseXML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (string k in Parameters.Keys)
            {
                string v = (string)Parameters[k];
                if (Regex.IsMatch(v, @"^[0-9.]$"))
                {

                    sb.Append("<" + k + ">" + v + "</" + k + ">");
                }
                else
                {
                    sb.Append("<" + k + "><![CDATA[" + v + "]]></" + k + ">");
                }

            }
            sb.Append("</xml>");
            return sb.ToString();
        }
    }

}

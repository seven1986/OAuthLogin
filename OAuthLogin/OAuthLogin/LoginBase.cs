using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web;

namespace OAuthLogin
{
    public class LoginBase
    {
        protected string AuthorizeCode
        {
            get
            {
                return HttpContext.Current.Request.QueryString["code"];
            }
        }

        /// <summary>
        /// 授权码回调地址
        /// </summary>
        protected string redirect_uri
        {
            get
            {
                return HttpContext.Current.Request.Url.AbsoluteUri.Split('?')[0];
            }
        }

        protected string Serialize(object obj)
        {
           return JsonConvert.SerializeObject(obj);
        }

        protected JObject Deserialize(string objStr)
        {
            return JsonConvert.DeserializeObject<JObject>(objStr);
        }

    }

    public class AuthorizeResult
    {
        public int code { get; set; }

        public string error { get; set; }

        public JObject result { get; set; }

        public string token { get; set; }
    }
}

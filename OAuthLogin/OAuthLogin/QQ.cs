using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace OAuthLogin
{
   public class QQ:LoginBase
    {

        string authorize_url = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id=" + LoginProvider.qq_client_id + "&redirect_uri=";

        string oauth_url = "https://graph.qq.com/oauth2.0/token?";

        string openid_url = "https://graph.qq.com/oauth2.0/me?access_token=";

        string user_info_url = "https://graph.qq.com/user/get_user_info?format=json&oauth_consumer_key=" + LoginProvider.qq_client_id
                                    + "&openid={0}&access_token={1}";

        public AuthorizeResult Authorize()
        {
            try
            {
                var code = AuthorizeCode;

                if (string.IsNullOrEmpty(code))
                {
                    HttpContext.Current.Response.Redirect(authorize_url + redirect_uri, true);

                    HttpContext.Current.Response.End();

                    return null;
                }

                if (!string.IsNullOrEmpty(code))
                {
                    var errorMsg = string.Empty;

                    var token = Accesstoken(code, ref errorMsg);

                    if (string.IsNullOrEmpty(errorMsg))
                    {
                        var access_token = token["access_token"];

                        var user = UserInfo(access_token, ref errorMsg);

                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            return new AuthorizeResult() { code = 0, result = user, token = access_token };
                        }

                        return new AuthorizeResult() { code = 3, error = errorMsg, token = access_token };
                    }

                    return new AuthorizeResult() { code = 2, error = errorMsg };
                }
            }

            catch (Exception ex)
            {
                return new AuthorizeResult() { code = 1, error = ex.Message };
            }

            return null;
        }

        private Dictionary<string, string> Accesstoken(string code, ref string errMsg)
        {
            var redirect_uri = HttpContext.Current.Request.Url.AbsoluteUri.Split('?')[0];
            var data = new SortedDictionary<string, string>();
            data.Add("client_id", LoginProvider.qq_client_id);
            data.Add("client_secret", LoginProvider.qq_client_secret);
            data.Add("grant_type", "authorization_code");
            data.Add("code", code);
            data.Add("redirect_uri", redirect_uri);

            var Params = string.Join("&", data.Select(x => x.Key + "=" + x.Value).ToArray());

            var AccessTokenUrl = oauth_url + Params;

            using (var wb = new WebClient())
            {
                try
                {
                    var result = wb.DownloadString(AccessTokenUrl);

                    var kvs = result.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

                    var dic = new Dictionary<string, string>();

                    foreach (var v in kvs)
                    {
                        var kv = v.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                        dic.Add(kv[0], kv[1]);
                    }

                    return dic;
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;

                    return null;
                }
            }
        }

        private JObject UserInfo(string token, ref string errMsg)
        {
            try
            {
                var result = string.Empty;

                using (var wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    result = wc.DownloadString(openid_url + token);
                }

                result = result.Replace("callback(", string.Empty).Replace(");", string.Empty).Trim();

                var openid = Deserialize(result).Value<string>("openid");

                using (var wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    result = wc.DownloadString(string.Format(user_info_url, openid, token));
                }

                var user = Deserialize(result);

                user.Add("openid", openid);

                return user;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;

                return null;
            }
        }
    }
}

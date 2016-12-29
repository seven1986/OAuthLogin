using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace OAuthLogin
{
   public class Wechat:LoginBase
    {
        string authorize_url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" +LoginProvider.Wechat_client_id + "&redirect_uri={0}&response_type=code&scope=snsapi_userinfo#wechat_redirect";

        string oauth_url = "https://api.weixin.qq.com/sns/oauth2/access_token?";

        string user_info_url = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";

        public AuthorizeResult Authorize()
        {
            try
            {
                var code = AuthorizeCode;

                if (string.IsNullOrEmpty(code))
                {
                    HttpContext.Current.Response.Redirect(string.Format(authorize_url,HttpUtility.UrlEncode(redirect_uri)), true);

                    HttpContext.Current.Response.End();

                    return null;
                }

                if (!string.IsNullOrEmpty(code))
                {
                    var errorMsg = string.Empty;

                    var token = Accesstoken(code, ref errorMsg);

                    if (string.IsNullOrEmpty(errorMsg))
                    {
                        var access_token = token.Value<string>("access_token");

                        var uid = token.Value<string>("openid");

                        var user = UserInfo(access_token, uid, ref errorMsg);

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

        private JObject Accesstoken(string code, ref string errMsg)
        {
            var data = new SortedDictionary<string, string>();
            data.Add("appid", LoginProvider.Wechat_client_id);
            data.Add("secret", LoginProvider.Wechat_client_secret);
            data.Add("grant_type", "authorization_code");
            data.Add("code", code);

            var Params = string.Join("&", data.Select(x => x.Key + "=" + x.Value).ToArray());

            var AccessTokenUrl = oauth_url + Params;

            using (var wb = new WebClient())
            {
                try
                {
                    var result = wb.UploadString(AccessTokenUrl, string.Empty);

                    return Deserialize(result);
                }

                catch (Exception ex)
                {
                    errMsg = ex.Message;

                    return null;
                }
            }
        }

        private JObject UserInfo(string token, string uid,ref string errMsg)
        {
            try
            {
                var result = string.Empty;

                using (var wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    result = wc.DownloadString(string.Format(user_info_url, token, uid));
                }

                var user = Deserialize(result);

                user.Add("uid", uid);

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

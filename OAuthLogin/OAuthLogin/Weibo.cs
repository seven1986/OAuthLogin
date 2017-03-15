using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace OAuthLogin
{
    public class Weibo : LoginBase
    {
        static string authorize_url = "https://api.weibo.com/oauth2/authorize?client_id=" + LoginProvider.Weibo_client_id + "&response_type=code&redirect_uri=";

        static string oauth_url = "https://api.weibo.com/oauth2/access_token?";

        static string user_info_url = "https://api.weibo.com/2/users/show.json?uid={0}&access_token={1}";

        public AuthorizeResult Authorize()
        {
            try
            {
                var code = AuthorizeCode;

                if (string.IsNullOrEmpty(code))
                {
                    HttpContext.Current.Response.Redirect(authorize_url + HttpUtility.UrlEncode(redirect_uri), true);

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

                        var uid = token.Value<string>("uid");

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
            data.Add("client_id", LoginProvider.Weibo_client_id);
            data.Add("client_secret", LoginProvider.Weibo_client_secret);
            data.Add("grant_type", "authorization_code");
            data.Add("code", code);
            data.Add("redirect_uri", redirect_uri);

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

        private JObject UserInfo(string token, string uid, ref string errMsg)
        {
            try
            {
                var result = string.Empty;

                using (var wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    result = wc.DownloadString(string.Format(user_info_url, uid, token));
                }

                var user = Deserialize(result);

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

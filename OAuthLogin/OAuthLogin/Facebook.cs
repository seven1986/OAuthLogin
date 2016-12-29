using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace OAuthLogin
{
   public class Facebook:LoginBase
    {
        string authorize_url = "https://www.facebook.com/v2.8/dialog/oauth?client_id=" + LoginProvider.FaceBook_client_id + "&scope=email,public_profile&response_type=code&redirect_uri=";

        string oauth_url = "https://graph.facebook.com/v2.8/oauth/access_token?";

        string user_info_url = "https://graph.facebook.com/me?fields=picture{url},name&access_token=";

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
                    var errMsg = string.Empty;

                    var token = Accesstoken(code, ref errMsg);

                    if (string.IsNullOrEmpty(errMsg))
                    {
                        var access_token = token.Value<string>("access_token");

                        var user = UserInfo(access_token, ref errMsg);

                        if (string.IsNullOrEmpty(errMsg))
                        {
                            return new AuthorizeResult() { code = 0, result = user,token=access_token };
                        }

                        return new AuthorizeResult() { code = 3, error = errMsg, token = access_token };
                    }

                    return new AuthorizeResult() { code = 2, error = errMsg };
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
            data.Add("client_id", LoginProvider.FaceBook_client_id);
            data.Add("client_secret", LoginProvider.FaceBook_client_secret);
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

        private JObject UserInfo(string token, ref string errMsg)
        {
            try
            {
                var result = string.Empty;

                using (var wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    result = wc.DownloadString(user_info_url + token);
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

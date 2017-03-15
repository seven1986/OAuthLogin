using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace OAuthLogin
{
   public class Kakao: LoginBase
    {
       static string authorize_url = "https://kauth.kakao.com/oauth/authorize?client_id=" + LoginProvider.Kakao_client_id + "&redirect_uri={0}&response_type=code";

       static string oauth_url = "https://kauth.kakao.com/oauth/token?";

       static string user_id_url = "https://kapi.kakao.com/v1/user/me";

       static string user_info_url = "https://kapi.kakao.com/v1/api/talk/profile";

        public AuthorizeResult Authorize()
        {
            try
            {
                var code = AuthorizeCode;

                if (string.IsNullOrEmpty(code))
                {
                    HttpContext.Current.Response.Redirect(string.Format(authorize_url, HttpUtility.UrlEncode(redirect_uri)), true);

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

                        var user = UserInfo(access_token,ref errorMsg);

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
            data.Add("client_id", LoginProvider.Kakao_client_id);
            data.Add("redirect_uri", redirect_uri);
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

        private JObject UserInfo(string token, ref string errMsg)
        {
            try
            {
                var result = string.Empty;

                using (var wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    wc.Headers.Add("Authorization", "Bearer " + token);

                    result = wc.DownloadString(user_info_url);
                }

                var user = Deserialize(result);

                using (var wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    wc.Headers.Add("Authorization", "Bearer " + token);

                    result = wc.DownloadString(user_id_url);
                }

                var userId = Deserialize(result);

                user["uid"] = userId["id"];

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

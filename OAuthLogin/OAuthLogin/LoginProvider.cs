namespace OAuthLogin
{
    public class LoginProvider
    {
        internal static string qq_client_id = string.Empty;
        internal static string qq_client_secret = string.Empty;
        public static void UseQQ(string client_id,string client_secret)
        {
            qq_client_id = client_id;
            qq_client_secret = client_secret;
        }

        internal static string Weibo_client_id = string.Empty;
        internal static string Weibo_client_secret = string.Empty;
        public static void UseWeibo(string client_id, string client_secret)
        {
            Weibo_client_id = client_id;
            Weibo_client_secret = client_secret;
        }

        internal static string Wechat_client_id = string.Empty;
        internal static string Wechat_client_secret = string.Empty;
        public static void UseWechat(string client_id, string client_secret)
        {
            Wechat_client_id = client_id;
            Wechat_client_secret = client_secret;
        }

        internal static string FaceBook_client_id = string.Empty;
        internal static string FaceBook_client_secret = string.Empty;
        public static void UseFaceBook(string client_id, string client_secret)
        {
            FaceBook_client_id = client_id;
            FaceBook_client_secret = client_secret;
        }

        internal static string Kakao_client_id = string.Empty;
        public static void UseKakao(string client_id)
        {
            Kakao_client_id = client_id;
        }
    }
}

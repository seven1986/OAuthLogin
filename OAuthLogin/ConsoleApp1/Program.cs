using OAuthLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LoginProvider.UseKakao("1bb20c677d5c689d69cbf2932ce10858");

            new Kakao().Authorize();

            Console.ReadLine();
        }
    }
}

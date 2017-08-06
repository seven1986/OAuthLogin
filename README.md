# OAuthLogin [![Build status](https://ci.appveyor.com/api/projects/status/bxsp9bee90r9pe8i?svg=true)](https://ci.appveyor.com/project/seven1986/oauthlogin-u3622) [![NuGet](https://img.shields.io/nuget/v/OAuthLogin.svg)](https://www.nuget.org/packages/OAuthLogin)  [![Join the chat at https://gitter.im/OAuthLogin/OAuthLogin](https://img.shields.io/gitter/room/OAuthLogin/OAuthLogin.svg?style=flat-square)](https://gitter.im/OAuthLogin/OAuthLogin?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) [![Average time to resolve an issue](http://isitmaintained.com/badge/resolution/seven1986/OAuthLogin.svg)](http://isitmaintained.com/project/seven1986/OAuthLogin "Average time to resolve an issue") [![GitHub license](https://img.shields.io/badge/license-Apache%202-blue.svg)](https://raw.githubusercontent.com/seven1986/OAuthLogin/master/LICENSE)

NuGet downloads (OAuthLogin) | NuGet downloads (OAuthLogin.AspNetCore)
--------------- | ---------------
[![NuGet downloads OAuthLogin](https://img.shields.io/nuget/dt/OAuthLogin.svg)](https://www.nuget.org/packages/OAuthLogin)|[![NuGet downloads OAuthLogin.AspNetCore](https://img.shields.io/nuget/dt/OAuthLogin.AspNetCore.svg)](https://www.nuget.org/packages/OAuthLogin.AspNetCore)

Installation
-------------

OAuthLogin is available as a NuGet package. You can install it using the NuGet Package Console window:

```
PM> Install-Package OAuthLogin
```


---

If you are looking for the ASP.NET Core version please head to [OAuthLogin.AspNetCore](https://github.com/seven1986/OAuthLogin.AspNetCore) project.

---

Usage
------

第一步：在Global.asax配置微博、微信、QQ、facebook、Kakao的client_id、client_secret

```csharp
protected void Application_Start(object sender, EventArgs e)
        {
            LoginProvider.UseFaceBook("client_id", "client_secret");

            LoginProvider.UseQQ("client_id", "client_secret");

            LoginProvider.UseWechat("client_id", "client_secret");

            LoginProvider.UseWeibo("client_id", "client_secret");

            LoginProvider.UseKakao("client_id");
        }
```

第二步：在项目根目录分别新建QQ.aspx、Wechat.aspx、Webo.aspx、Facebook.aspx文件

##### QQ.aspx

```csharp
 protected void Page_Load(object sender, EventArgs e)
        {
            var res = new QQ().Authorize();

            if (res != null && res.code == 0)
            {
                //拿到结果数据，然后进行自定义跳转
                //res.result
            }
        }
```

##### Wechat.aspx

```csharp
protected void Page_Load(object sender, EventArgs e)
        {
            var res = new Wechat().Authorize();

            if (res != null && res.code == 0)
            {
                //拿到结果数据，然后进行自定义跳转
                //res.result
            }
        }
```

##### Webo.aspx

```csharp
protected void Page_Load(object sender, EventArgs e)
        {
            var res = new Weibo().Authorize();

            if (res != null && res.code == 0)
            {
                //拿到结果数据，然后进行自定义跳转
                //res.result
            }
        }
```

##### Facebook.aspx

```csharp
protected void Page_Load(object sender, EventArgs e)
        {
            var res = new Facebook().Authorize();

            if (res != null && res.code==0)
            {
                //拿到结果数据，然后进行自定义跳转
                //res.result
            }
        }
```

##### Kakao.aspx

```csharp
protected void Page_Load(object sender, EventArgs e)
        {
            var res = new Kakao().Authorize();

            if (res != null&& res.code==0)
            {
                //拿到结果数据，然后进行自定义跳转
                //res.result
            }
        }
```

using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Alfred.Util;
using Alfred.Util.Extension;
using Alfred.Cache.Factory;

namespace Alfred.Web.Code
{
    public class Operator
    {
        public static Operator Instance
        {
            get { return new Operator(); }
        }
        //该字段使用GlobalContext.Configuration.GetSection方法获取名为"SystemConfig:LoginProvider"的配置项的值，并将其赋给LoginProvider字段
        private string LoginProvider = GlobalContext.Configuration.GetSection("SystemConfig:LoginProvider").Value;
        private string TokenName = "UserToken"; //cookie name or session name

        public async Task AddCurrent(string token)
        {
            switch (LoginProvider)
            {
                case "Cookie":
                    new CookieHelper().WriteCookie(TokenName, token);
                    break;

                case "Session":
                    new SessionHelper().WriteSession(TokenName, token);
                    break;

                case "WebApi":
                    OperatorInfo user = await new DataRepository().GetUserByToken(token);
                    if (user != null)
                    {
                        CacheFactory.Cache.SetCache(token, user);
                    }
                    break;

                default:
                    throw new Exception("未找到LoginProvider配置");
            }
        }

        /// <summary>
        /// Api接口需要传入apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        public void RemoveCurrent(string apiToken = "")
        {
            switch (LoginProvider)
            {
                case "Cookie":
                    new CookieHelper().RemoveCookie(TokenName);
                    break;

                case "Session":
                    new SessionHelper().RemoveSession(TokenName);
                    break;

                case "WebApi":
                    CacheFactory.Cache.RemoveCache(apiToken);
                    break;

                default:
                    throw new Exception("未找到LoginProvider配置");
            }
        }

        /// <summary>
        /// Api接口需要传入apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        /// 根据方法名和参数的含义，可以猜测该方法的作用是获取当前操作员的信息。可能会使用传递的API令牌来验证操作员的身份，并返回包含操作员信息的OperatorInfo对象。
        public async Task<OperatorInfo> Current(string apiToken = "")
        {
            //以上代码使用了ASP.NET Core提供的IHttpContextAccessor接口来获取当前HTTP上下文信息。
            //首先，通过ServiceProvider属性获取全局的服务提供程序（ServiceProvider）。然后，通过GetService方法获取IHttpContextAccessor服务。
            //GetService方法返回一个对象，需要用问号进行null条件运算符的判断，以处理ServiceProvider为null的情况。
            //最后，将获取到的IHttpContextAccessor对象赋值给hca变量。
            //该代码片段的作用是获取当前请求的HTTP上下文信息，可以通过该对象获取请求的相关信息，如请求头部、Cookies、请求路径等。

            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            OperatorInfo user = null;
            string token = string.Empty;
            switch (LoginProvider)
            {
                case "Cookie":
                    if (hca.HttpContext != null)
                    {
                        token = new CookieHelper().GetCookie(TokenName);
                    }
                    break;

                case "Session":
                    if (hca.HttpContext != null)
                    {
                        token = new SessionHelper().GetSession(TokenName);
                    }
                    break;

                case "WebApi":
                    token = apiToken;
                    break;
            }
            if (string.IsNullOrEmpty(token))
            {
                return user;
            }
            token = token.Trim('"');
            user = CacheFactory.Cache.GetCache<OperatorInfo>(token);
            if (user == null)
            {
                user = await new DataRepository().GetUserByToken(token);
                if (user != null)
                {
                    CacheFactory.Cache.SetCache(token, user);
                }
            }
            return user;
        }
    }
}

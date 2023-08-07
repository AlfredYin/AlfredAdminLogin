using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;

namespace Alfred.Util
{
    public class SessionHelper
    {
        /// <summary>
        /// 写Session
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public void WriteSession<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public void WriteSession(string key, string value)
        {
            WriteSession<string>(key, value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public string GetSession(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            /*
             这一行代码定义了一个名为hca的变量，类型为IHttpContextAccessor。
            它使用了GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>()方法来获取一个实现了IHttpContextAccessor接口的服务。
            首先，GlobalContext.ServiceProvider是一个IServiceProvider类型的属性或字段，用于获取应用程序中的服务提供程序。该服务提供程序是一个容器，它包含了应用程序中注册的各种服务。
            然后通过调用GetService<IHttpContextAccessor>方法，从服务提供程序中查找并返回实现了IHttpContextAccessor接口的服务对象。
            这个接口用于提供对HTTP上下文的访问，包括请求、响应、用户信息等。
            最后，使用null条件运算符（?）来处理可能的空引用异常。
            如果GlobalContext.ServiceProvider为null，则整个表达式返回null，否则继续执行GetService<IHttpContextAccessor>()方法来获取服务对象。
            据这段代码的用途推测，hca变量可能被用于获取和操作HTTP上下文的相关信息。
             */
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();

            //---debug 通过Session获取的 验证码的值为4
            string a= hca?.HttpContext?.Session.GetString(key) as string;

            return hca?.HttpContext?.Session.GetString(key) as string;
        }

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public void RemoveSession(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Session.Remove(key);
        }
    }
}

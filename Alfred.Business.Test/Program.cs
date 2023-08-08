using Alfred.Util;
using Alfred.Util.Model;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Alfred.Business.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("D:\\Desktop\\.net后端\\AlfredAdminLogin\\Alfred.Business.Test\\appsettings.json")
                .Build();
            //数据对象初始化
            GlobalContext.SystemConfig = configuration.GetSection("SystemConfig").Get<SystemConfig>();

            Program.Test();

            Thread.Sleep(100000);

            Console.WriteLine("Hello, World!");
        }

        public static async Task<int> Test()
        {
            TestBLL testBLL = new TestBLL();

            var list = await testBLL.GetList();

            return list.Count;
        }
    }
}
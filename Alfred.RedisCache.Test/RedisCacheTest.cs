using Alfred.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Util.Model;
using Alfred.Cache.Factory;

namespace Alfred.RedisCache.Test
{
    public class RedisCacheTest
    {
        //初始化连接,数据库和Redis
        public void Init()
        {
            GlobalContext.SystemConfig = new SystemConfig
            {
                DBProvider = "SqlServer",
                DBConnectionString = "Server=localhost;DataBase=test_3;Uid=sa;Pwd=Yin1719934825;TrustServerCertificate=true;",

                CacheProvider = "Redis",
                RedisConnectionString = "127.0.0.1:6379"
            };
        }

        public void TestRedisSimple()
        {
            string key = "test_simple_key";
            string value = "test_simple_value";
            //

            //public bool SetCache<T>(string key, T value, DateTime? expireTime = null)
            CacheFactory.Cache.SetCache<string> (key, value);

            //存入很多测试数值
            string key1 = "a";
            int value1 = 1;
            for(int i = 0; i < 10;i++) 
            { 
                key1=key1+i;
                value1=value1+i;
                CacheFactory.Cache.SetCache<int>(key1, value1);
            }

            //测试是否返回了正常值
            Console.WriteLine("value = " +CacheFactory.Cache.GetCache<string>(key));

        }

        public void TestRedisComple()
        {
            string key = "test_complex_key";
            TData<string> value = new TData<string> { Tag = 1, Data = "测试Redis" };
            CacheFactory.Cache.SetCache<TData<string>>(key, value);

            var resultRedis = CacheFactory.Cache.GetCache<TData<string>>(key);

            //测试是否返回了正确值
            Console.WriteLine("value= "+ resultRedis.Data);
        }

        /// <summary>
        /// 对Redis的性能测试
        /// </summary>
        /// <returns></returns>
        public async Task TestRedisPerformance()
        {

        }
    }
}

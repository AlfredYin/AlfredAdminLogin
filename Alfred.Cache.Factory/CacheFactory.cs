using Alfred.Util;
using Alfred.Cache.Interface;
using Alfred.MemoryCache;
using Alfred.RedisCache;

namespace Alfred.Cache.Factory
{
    /// <summary>
    /// 缓存工厂  --- MemoryCache 或者 Redis 实现
    /// </summary>
    public class CacheFactory
    {
        private static ICache cache = null;
        private static readonly object lockHelper = new object();

        public static ICache Cache
        {
            get
            {
                if (cache == null)
                {
                    lock (lockHelper)
                    {
                        if (cache == null)
                        {
                            switch (GlobalContext.SystemConfig.CacheProvider)
                            {
                                //如果是Redis,使用ICache接口实现 RedisCache实现类
                                case "Redis": cache = new RedisCacheImp(); break;

                                default:
                                case "Memory": cache = new MemoryCacheImp(); break;
                            }
                        }
                    }
                }
                return cache;
            }
        }
    }
}

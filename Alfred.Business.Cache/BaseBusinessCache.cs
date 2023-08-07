using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Cache.Factory;

namespace Alfred.Business.Cache
{
    //BaseBusinessCache<T>是一个抽象类，用于封装业务缓存操作 基类需要继承
    public abstract class BaseBusinessCache<T>
    {
        //表示缓存的唯一标识,需要在子类中实现,通常是个字符串
        public abstract string CacheKey { get; }

        //用于从缓存中移除该缓存项。默认实现是调用CacheFactory.Cache.RemoveCache()方法，传入缓存键值，并返回删除操作是否成功
        public virtual bool Remove()
        {
            return CacheFactory.Cache.RemoveCache(CacheKey);
        }

        //获取列表
        public virtual Task<List<T>> GetList()
        {
            throw new Exception("请在子类实现");
        }
    }
}

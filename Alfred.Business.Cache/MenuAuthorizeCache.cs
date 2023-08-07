using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alfred.Cache.Factory;
using Alfred.Entity.SystemManage;
using Alfred.Service.SystemManage;

namespace Alfred.Business.Cache
{
    public class MenuAuthorizeCache : BaseBusinessCache<MenuAuthorizeEntity>
    {
        //实例化MenuAuthorizeService 授权菜单的服务
        private MenuAuthorizeService menuAuthorizeService = new MenuAuthorizeService();

        //在子类中，使用this.GetType().Name可以获取到子类的类型名称。可以将其作为缓存键值的一部分，以确保缓存的唯一性。
        public override string CacheKey => this.GetType().Name;

        //重写GetList
        public override async Task<List<MenuAuthorizeEntity>> GetList()
        {
            //获取已经存储的Cache,为空的话返回nulll
            var cacheList = CacheFactory.Cache.GetCache<List<MenuAuthorizeEntity>>(CacheKey);

            if (cacheList == null)
            {
                //定义list,并将该list存入Cache
                var list = await menuAuthorizeService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);

                //返回list
                return list;
            }
            else
            {
                //返回cache中的List
                return cacheList;
            }
        }
    }
}

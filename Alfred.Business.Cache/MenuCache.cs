using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alfred.Cache.Factory;
using Alfred.Entity.SystemManage;
using Alfred.Service.SystemManage;

namespace Alfred.Business.Cache
{
    public class MenuCache : BaseBusinessCache<MenuEntity>
    {
        private MenuService menuService = new MenuService();

        public override string CacheKey => this.GetType().Name;

        //异步函数,获取菜单实体
        public override async Task<List<MenuEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<MenuEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await menuService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }
    }
}

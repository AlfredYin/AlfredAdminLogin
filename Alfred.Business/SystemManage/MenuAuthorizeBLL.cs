using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Business.Cache;
using Alfred.Business.OrganizationManage;
using Alfred.Cache.Factory;
using Alfred.Entity.OrganizationManage;
using Alfred.Entity.SystemManage;
using Alfred.Enum;
using Alfred.Enum.SystemManage;
using Alfred.Model.Result;
using Alfred.Service.SystemManage;
using Alfred.Util.Extension;
using Alfred.Util.Model;
using Alfred.Web.Code;

namespace Alfred.Business.SystemManage
{
    public class MenuAuthorizeBLL
    {
        private MenuAuthorizeCache menuAuthorizeCache = new MenuAuthorizeCache();
        private MenuCache menuCache = new MenuCache();

        #region 获取数据

        //使用用户身份信息获取授权的菜单
        //接下来，通过调用MenuAuthorizeBLL类的GetAuthorizeList方法获取授权的菜单，并将结果保存在一个TData<List<MenuAuthorizeInfo>>对象中
        public async Task<TData<List<MenuAuthorizeInfo>>> GetAuthorizeList(OperatorInfo user)
        {
            /*
             * TData类一般包含以下几个属性：
        Tag：表示操作结果状态，一般使用整数来表示。比如，1代表成功，0代表失败。
        Message：表示操作结果的提示信息，一般是字符串类型。
        Description: 描述
            
            TData<T> : 这个泛型类 继承 TData类
        添加属性: int Total;
            T Data;   泛型的Data数据
             * 
             *  public class MenuAuthorizeInfo
                {
                    //菜单的ID
                    [JsonConverter(typeof(StringJsonConverter))]
                    public long? MenuId { get; set; }
                    
                    /// <summary>
                    /// 用户Id或者角色Id
                    /// </summary>
                    [JsonConverter(typeof(StringJsonConverter))]
                    public long? AuthorizeId { get; set; }
            
                    /// <summary>
                    ///  用户或者角色
                    /// </summary>
                    public int? AuthorizeType { get; set; }

                    /// <summary>
                    /// 权限标识
                    /// </summary>
                    public string Authorize { get; set; }
                }
             */
            //TData泛型类,数据格式为List<MenuAuthorizeInfo>的MenuAuthorizeInfo的列表
            //泛型类实例化
            TData<List<MenuAuthorizeInfo>> obj = new TData<List<MenuAuthorizeInfo>>();
            //泛型类的Data实例化,即List<MenuAuthorizeInfo>实例化
            obj.Data = new List<MenuAuthorizeInfo>();

            /*
             * [Table("SysMenuAuthorize")]
                public class MenuAuthorizeEntity : BaseCreateEntity
                {
                    [JsonConverter(typeof(StringJsonConverter))]
                    public long? MenuId { get; set; }

                    [JsonConverter(typeof(StringJsonConverter))]
                    public long? AuthorizeId { get; set; }

                    public int? AuthorizeType { get; set; }

                    [NotMapped]
                    public string AuthorizeIds { get; set; }
                }
             */
            //授权菜单的实体  --- 列表
            //授权菜单列表
            List<MenuAuthorizeEntity> authorizeList = new List<MenuAuthorizeEntity>();
            //授权用户列表
            List<MenuAuthorizeEntity> userAuthorizeList = null;
            //授权角色列表
            List<MenuAuthorizeEntity> roleAuthorizeList = null;

            //使用Cache 获取授权菜单的Cache列表,如果Memory/Redis中没有,则 var list = await menuAuthorizeService.GetList(null); CacheFactory.Cache.SetCache(CacheKey, list);
            //这肯定是获取授权菜单Cache列表
            var menuAuthorizeCacheList = await menuAuthorizeCache.GetList();

            //使用Cache 获取未授权的MenuList
            var menuList = await menuCache.GetList();

            //enableMenuIdList 元素Id  
            //Where方法对menuList进行过滤，只保留满足条件p.MenuStatus == (int)StatusEnum.Yes的元素 即[Description("启用")],也即开启使用的菜单
            //Select方法将过滤后的元素中的Id属性提取出来，形成一个新的集合。 仅筛选出 行中的Id元素出来.
            //ToList方法将新的集合转换为List<int>对象，并作为方法的返回值 枚举StatusEnum.Yes=1
            var enableMenuIdList = menuList.Where(p => p.MenuStatus == (int)StatusEnum.Yes).Select(p => p.Id).ToList();

            //获取授权的menuCache列表中获取开启的使用的菜单
            menuAuthorizeCacheList = menuAuthorizeCacheList.Where(p => enableMenuIdList.Contains(p.MenuId)).ToList();

            // ==用户授权ID 权限判断类型==User
            userAuthorizeList = menuAuthorizeCacheList.Where(p => p.AuthorizeId == user.UserId && p.AuthorizeType == AuthorizeTypeEnum.User.ParseToInt()).ToList();

            // 角色判断
            if (!string.IsNullOrEmpty(user.RoleIds))
            {
                List<long> roleIdList = user.RoleIds.Split(',').Select(p => long.Parse(p)).ToList();
                roleAuthorizeList = menuAuthorizeCacheList.Where(p => roleIdList.Contains(p.AuthorizeId.Value) && p.AuthorizeType == AuthorizeTypeEnum.Role.ParseToInt()).ToList();
            }

            // 排除重复的记录
            if (userAuthorizeList.Count > 0)
            {
                authorizeList.AddRange(userAuthorizeList);
                roleAuthorizeList = roleAuthorizeList.Where(p => !userAuthorizeList.Select(u => u.AuthorizeId).Contains(p.AuthorizeId)).ToList();
            }
            if (roleAuthorizeList != null && roleAuthorizeList.Count > 0)
            {
                authorizeList.AddRange(roleAuthorizeList);
            }

            //加入返回的obj中
            foreach (MenuAuthorizeEntity authorize in authorizeList)
            {
                obj.Data.Add(new MenuAuthorizeInfo
                {
                    MenuId = authorize.MenuId,
                    AuthorizeId = authorize.AuthorizeId,
                    AuthorizeType = authorize.AuthorizeType,
                    Authorize = menuList.Where(t => t.Id == authorize.MenuId).Select(t => t.Authorize).FirstOrDefault()
                });
            }
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}

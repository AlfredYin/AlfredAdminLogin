﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Business.Cache;
using Alfred.Business.SystemManage;
using Alfred.Cache.Factory;
using Alfred.Entity;
using Alfred.Entity.OrganizationManage;
using Alfred.Entity.SystemManage;
using Alfred.Enum;
using Alfred.Enum.OrganizationManage;
using Alfred.Model;
using Alfred.Model.Param;
using Alfred.Model.Param.OrganizationManage;
using Alfred.Service.OrganizationManage;
using Alfred.Util;
using Alfred.Util.Extension;
using Alfred.Util.Model;
using Alfred.Web.Code;

namespace Alfred.Business.OrganizationManage
{
    public class UserBLL
    {
        private UserService userService = new UserService();
        private UserBelongService userBelongService = new UserBelongService();
        private DepartmentService departmentService = new DepartmentService();

        private DepartmentBLL departmentBLL = new DepartmentBLL();

        #region 获取数据
        public async Task<TData<List<UserEntity>>> GetList(UserListParam param)
        {
            TData<List<UserEntity>> obj = new TData<List<UserEntity>>();
            obj.Data = await userService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        //获取员工信息的列表
        //GetPageList
        /*首先，定义一个类型为TData<List<UserEntity>>的泛型对象obj，用于存储返回结果。
        接着，判断参数param中的DepartmentId是否存在。
        如果存在，则调用departmentBLL.GetChildrenDepartmentIdList(null, param.DepartmentId.Value)方法来获取子部门列表，
        并将结果赋值给param.ChildrenDepartmentIdList。如果不存在，则通过Operator.Instance.Current()方法获取当前登录用户的信息，
        并调用departmentBLL.GetChildrenDepartmentIdList(null, user.DepartmentId.Value)方法来获取子部门列表，结果同样赋值给param.ChildrenDepartmentIdList。
        */
        public async Task<TData<List<UserEntity>>> GetPageList(UserListParam param, Pagination pagination)
        {
            //定义一个类型为TData<List<UserEntity>>的泛型对象obj，用于存储返回结果
            TData<List<UserEntity>> obj = new TData<List<UserEntity>>();

            //接着，判断参数param中的DepartmentId是否存在,相当于是总机构的节点.
            if (param?.DepartmentId != null)
            {
                //如果存在，则调用departmentBLL.GetChildrenDepartmentIdList(null, param.DepartmentId.Value)方法来获取子部门列表
                param.ChildrenDepartmentIdList = await departmentBLL.GetChildrenDepartmentIdList(null, param.DepartmentId.Value);
            }
            else
            {
                //不存在,获取用户信息,使用该用户信息,获取属下的所有子部门列表
                OperatorInfo user = await Operator.Instance.Current();
                param.ChildrenDepartmentIdList = await departmentBLL.GetChildrenDepartmentIdList(null, user.DepartmentId.Value);
            }
            //调用userService.GetPageList(param, pagination)方法来获取用户分页列表，并将结果赋值给obj.Data
            obj.Data = await userService.GetPageList(param, pagination);
            //调用userBelongService.GetList(new UserBelongEntity { UserIds = obj.Data.Select(p => p.Id.Value).ParseToStrings<long>() })方法来获取用户所属关系列表，
            //并将结果赋值给userBelongList
            //用户所属关系列表
            /*
             * [Table("SysUserBelong")]
                public class UserBelongEntity : BaseCreateEntity
                {
                    [JsonConverter(typeof(StringJsonConverter))]
                    public long? UserId { get; set; }
                    [JsonConverter(typeof(StringJsonConverter))]
                    public long? BelongId { get; set; }
                    public int? BelongType { get; set; }
                
                    /// <summary>
                    /// 多个用户Id
                    /// </summary>
                    [NotMapped]
                    public string UserIds { get; set; }
                }
             * 
             *  public async Task<List<UserBelongEntity>> GetList(UserBelongEntity entity)  //参数是UserBelongEntity的实体
                {
                    var expression = LinqExtensions.True<UserBelongEntity>();
                    if (entity != null)
                    {
                        if (entity.BelongType != null)
                        {
                            expression = expression.And(t => t.BelongType == entity.BelongType);
                        }
                        if (entity.UserId != null)
                        {
                            expression = expression.And(t => t.UserId == entity.UserId);
                        }
                    }
                    var list = await this.BaseRepository().FindList(expression);
                    return list.ToList();
                }
             */

            List<UserBelongEntity> userBelongList = await userBelongService.GetList(new UserBelongEntity { UserIds = obj.Data.Select(p => p.Id.Value).ParseToStrings<long>() });
            //调用departmentService.GetList(new DepartmentListParam { Ids = userBelongList.Select(p => p.BelongId.Value).ParseToStrings<long>() })方法来获取部门列表，
            //并将结果赋值给departmentList。
            //部门列表
            List<DepartmentEntity> departmentList = await departmentService.GetList(new DepartmentListParam { Ids = userBelongList.Select(p => p.BelongId.Value).ParseToStrings<long>() });
            //然后，通过循环遍历obj.Data中的每个用户，为每个用户的DepartmentName属性赋值。
            //该属性值是根据departmentList中与用户DepartmentId匹配的部门记录的DepartmentName属性进行赋值。
            foreach (UserEntity user in obj.Data)
            {
                user.DepartmentName = departmentList.Where(p => p.Id == user.DepartmentId).Select(p => p.DepartmentName).FirstOrDefault();
            }
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<UserEntity>> GetEntity(long id)
        {
            TData<UserEntity> obj = new TData<UserEntity>();
            obj.Data = await userService.GetEntity(id);

            await GetUserBelong(obj.Data);

            if (obj.Data.DepartmentId > 0)
            {
                DepartmentEntity departmentEntity = await departmentService.GetEntity(obj.Data.DepartmentId.Value);
                if (departmentEntity != null)
                {
                    obj.Data.DepartmentName = departmentEntity.DepartmentName;
                }
            }

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<UserEntity>> CheckLogin(string userName, string password, int platform)
        {
            TData<UserEntity> obj = new TData<UserEntity>();
            if (userName.IsEmpty() || password.IsEmpty())
            {
                obj.Message = "用户名或密码不能为空";
                return obj;
            }
            UserEntity user = await userService.CheckLogin(userName);
            if (user != null)
            {
                if (user.UserStatus == (int)StatusEnum.Yes)
                {
                    if (user.Password == EncryptUserPassword(password, user.Salt))
                    {
                        user.LoginCount++;
                        user.IsOnline = 1;

                        #region 设置日期
                        if (user.FirstVisit == GlobalConstant.DefaultTime)
                        {
                            user.FirstVisit = DateTime.Now;
                        }
                        if (user.PreviousVisit == GlobalConstant.DefaultTime)
                        {
                            user.PreviousVisit = DateTime.Now;
                        }
                        if (user.LastVisit != GlobalConstant.DefaultTime)
                        {
                            user.PreviousVisit = user.LastVisit;
                        }
                        user.LastVisit = DateTime.Now;
                        #endregion

                        switch (platform)
                        {
                            case (int)PlatformEnum.Web:
                                if (GlobalContext.SystemConfig.LoginMultiple)
                                {
                                    #region 多次登录用同一个token
                                    if (string.IsNullOrEmpty(user.WebToken))
                                    {
                                        user.WebToken = SecurityHelper.GetGuid(true);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    user.WebToken = SecurityHelper.GetGuid(true);
                                }
                                break;

                            case (int)PlatformEnum.WebApi:
                                user.ApiToken = SecurityHelper.GetGuid(true);
                                break;
                        }
                        await GetUserBelong(user);

                        obj.Data = user;
                        obj.Message = "登录成功";
                        obj.Tag = 1;
                    }
                    else
                    {
                        obj.Message = "密码不正确，请重新输入";
                    }
                }
                else
                {
                    obj.Message = "账号被禁用，请联系管理员";
                }
            }
            else
            {
                obj.Message = "账号不存在，请重新输入";
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(UserEntity entity)
        {
            TData<string> obj = new TData<string>();
            if (userService.ExistUserName(entity))
            {
                obj.Message = "用户名已经存在！";
                return obj;
            }
            if (entity.Id.IsNullOrZero())
            {
                entity.Salt = GetPasswordSalt();
                entity.Password = EncryptUserPassword(entity.Password, entity.Salt);
            }
            if (!entity.Birthday.IsEmpty())
            {
                entity.Birthday = entity.Birthday.ParseToDateTime().ToString("yyyy-MM-dd");
            }
            await userService.SaveForm(entity);

            await RemoveCacheById(entity.Id.Value);

            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            if (string.IsNullOrEmpty(ids))
            {
                obj.Message = "参数不能为空";
                return obj;
            }
            await userService.DeleteForm(ids);

            await RemoveCacheById(ids);

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<long>> ResetPassword(UserEntity entity)
        {
            TData<long> obj = new TData<long>();

            //为啥判断大于0?
            if (entity.Id > 0)
            {
                //通过Id 获取要修改的实体
                //数据库实体
                UserEntity dbUserEntity = await userService.GetEntity(entity.Id.Value);

                //数据库实体与参数实体进行比较
                if (dbUserEntity.Password == entity.Password)
                {
                    obj.Message = "密码未更改";
                    return obj;
                }

                //获取加密盐
                entity.Salt = GetPasswordSalt();
                //通过加密盐计算密码
                entity.Password = EncryptUserPassword(entity.Password, entity.Salt);

                //userService更改密码
                await userService.ResetPassword(entity);

                //删除Cache,通过实体的Id删除
                await RemoveCacheById(entity.Id.Value);

                //返回值填充
                obj.Data = entity.Id.Value;
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<long>> ChangePassword(ChangePasswordParam param)
        {
            TData<long> obj = new TData<long>();
            if (param.Id > 0)
            {
                if (string.IsNullOrEmpty(param.Password) || string.IsNullOrEmpty(param.NewPassword))
                {
                    obj.Message = "新密码不能为空";
                    return obj;
                }
                UserEntity dbUserEntity = await userService.GetEntity(param.Id.Value);
                if (dbUserEntity.Password != EncryptUserPassword(param.Password, dbUserEntity.Salt))
                {
                    obj.Message = "旧密码不正确";
                    return obj;
                }
                dbUserEntity.Salt = GetPasswordSalt();
                dbUserEntity.Password = EncryptUserPassword(param.NewPassword, dbUserEntity.Salt);
                await userService.ResetPassword(dbUserEntity);

                await RemoveCacheById(param.Id.Value);

                obj.Data = dbUserEntity.Id.Value;
                obj.Tag = 1;
            }
            return obj;
        }

        /// <summary>
        /// 用户自己修改自己的信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TData<long>> ChangeUser(UserEntity entity)
        {
            TData<long> obj = new TData<long>();
            if (entity.Id > 0)
            {
                await userService.ChangeUser(entity);

                await RemoveCacheById(entity.Id.Value);

                obj.Data = entity.Id.Value;
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData> UpdateUser(UserEntity entity)
        {
            TData obj = new TData();
            await userService.UpdateUser(entity);

            obj.Tag = 1;
            return obj;
        }
        /// <summary>
        /// 导入execl数据
        /// </summary>
        /// <param name="param"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<TData> ImportUser(ImportParam param, List<UserEntity> list)
        {
            TData obj = new TData();
            if (list.Any())
            {
                foreach (UserEntity entity in list)
                {
                    UserEntity dbEntity = await userService.GetEntity(entity.UserName);
                    if (dbEntity != null)
                    {
                        entity.Id = dbEntity.Id;
                        if (param.IsOverride == 1)
                        {
                            await userService.SaveForm(entity);
                            await RemoveCacheById(entity.Id.Value);
                        }
                    }
                    else
                    {
                        await userService.SaveForm(entity);
                        await RemoveCacheById(entity.Id.Value);
                    }
                }
                obj.Tag = 1;
            }
            else
            {
                obj.Message = " 未找到导入的数据";
            }
            return obj;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 密码MD5处理
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string EncryptUserPassword(string password, string salt)
        {
            string md5Password = SecurityHelper.MD5ToHex(password);
            string encryptPassword = SecurityHelper.MD5ToHex(md5Password.ToLower() + salt).ToLower();
            return encryptPassword;
        }

        /// <summary>
        /// 密码盐
        /// </summary>
        /// <returns></returns>
        private string GetPasswordSalt()
        {
            return new Random().Next(1, 100000).ToString();
        }

        /// <summary>
        /// 移除缓存里面的token
        /// </summary>
        /// <param name="id"></param>
        private async Task RemoveCacheById(string ids)
        {
            foreach (long id in ids.Split(',').Select(p => long.Parse(p)))
            {
                await RemoveCacheById(id);
            }
        }

        /// <summary>
        /// 通过Id删除,缓存中的实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task RemoveCacheById(long id)
        {
            var dbEntity = await userService.GetEntity(id);
            if (dbEntity != null)
            {
                CacheFactory.Cache.RemoveCache(dbEntity.WebToken);
            }
        }

        /// <summary>
        /// 获取用户的职位和角色
        /// </summary>
        /// <param name="user"></param>
        private async Task GetUserBelong(UserEntity user)
        {
            List<UserBelongEntity> userBelongList = await userBelongService.GetList(new UserBelongEntity { UserId = user.Id });

            List<UserBelongEntity> roleBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Role.ParseToInt()).ToList();
            if (roleBelongList.Count > 0)
            {
                user.RoleIds = string.Join(",", roleBelongList.Select(p => p.BelongId).ToList());
            }

            List<UserBelongEntity> positionBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Position.ParseToInt()).ToList();
            if (positionBelongList.Count > 0)
            {
                user.PositionIds = string.Join(",", positionBelongList.Select(p => p.BelongId).ToList());
            }
        }
        #endregion
    }
}

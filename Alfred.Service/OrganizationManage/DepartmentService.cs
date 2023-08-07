using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Alfred.Data.Repository;
using Alfred.Entity.OrganizationManage;
using Alfred.Model;
using Alfred.Model.Param.OrganizationManage;
using Alfred.Util;
using Alfred.Util.Extension;
using Alfred.Util.Model;

namespace Alfred.Service.OrganizationManage
{
    public class DepartmentService : RepositoryFactory
    {
        #region 获取数据
        //这个方法定义了一个异步方法GetList，返回类型是Task<List<DepartmentEntity>>。方法接受一个名为param的参数，类型是DepartmentListParam。
        //返回获取到的列表
        public async Task<List<DepartmentEntity>> GetList(DepartmentListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.OrderBy(p => p.DepartmentSort).ToList();
        }

        public async Task<DepartmentEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DepartmentEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(DepartmentSort) FROM SysDepartment");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        /// <summary>
        /// 部门名称是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExistDepartmentName(DepartmentEntity entity)
        {
            var expression = LinqExtensions.True<DepartmentEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.DepartmentName == entity.DepartmentName && t.ParentId == entity.ParentId);
            }
            else
            {
                expression = expression.And(t => t.DepartmentName == entity.DepartmentName && t.ParentId == entity.ParentId && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }

        /// <summary>
        /// 是否存在子部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistChildrenDepartment(long id)
        {
            var expression = LinqExtensions.True<DepartmentEntity>();
            expression = expression.And(t => t.ParentId == id);
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(DepartmentEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                await entity.Modify();
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            var db = await this.BaseRepository().BeginTrans();
            try
            {
                long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                await db.Delete<DepartmentEntity>(idArr);
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }
        #endregion

        #region 私有方法
        //这个方法定义了一个私有方法ListFilter，返回类型是Expression<Func<DepartmentEntity, bool>>。方法接受一个名为param的参数，类型是DepartmentListParam。
        //这个方法的作用是根据传入的参数生成一个条件表达式，用于筛选部门列表。如果param参数不为空，且DepartmentName属性不为空，会生成一个根据DepartmentName属性进行模糊查询的表达式。
        private Expression<Func<DepartmentEntity, bool>> ListFilter(DepartmentListParam param)
        {
            //在方法中，首先创建一个expression表达式，使用LinqExtensions.True<DepartmentEntity>()方法初始化为一个始终为true的表达式。
            var expression = LinqExtensions.True<DepartmentEntity>();
            if (param != null)
            {
                //然后判断传入的param参数是否为空。如果不为空，则判断DepartmentName属性是否为空。
                //如果不为空，则使用expression.And()方法拼接一个根据DepartmentName属性进行模糊查询的表达式
                if (!param.DepartmentName.IsEmpty())
                {
                    expression = expression.And(t => t.DepartmentName.Contains(param.DepartmentName));
                }
            }
            return expression;
        }
        #endregion
    }
}

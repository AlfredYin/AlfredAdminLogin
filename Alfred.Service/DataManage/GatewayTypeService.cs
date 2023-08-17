using System.Linq.Expressions;
using Alfred.Data.Repository;
using Alfred.Util.Extension;
using Alfred.Entity.GwDataManage;
using Alfred.Model.Param.GatewayDataManage;

namespace Alfred.Service.GatewayDataManage
{
    public class GatewayTypeService:RepositoryFactory
    {
        #region 获取数据
        public async Task<List<GatewayTypeEntity>> GetList(GatewayTypeParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.OrderBy(p=>p.GatewayTypeSort).ToList();
        }
        public async Task<GatewayTypeEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<GatewayTypeEntity>(id);
        }
        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(GatewayTypeSort) FROM SysGatewayType");
            int sort=result.ParseToInt();
            sort++;
            return sort;
        }
        #endregion

        #region 私有方法
        //部门查询 模糊查询
        private Expression<Func<GatewayTypeEntity, bool>> ListFilter(GatewayTypeParam param)
        {
            var expression = LinqExtensions.True<GatewayTypeEntity>();
            if (param != null)
            {
                //然后判断传入的param参数是否为空。如果不为空，则判断DepartmentName属性是否为空。
                //如果不为空，则使用expression.And()方法拼接一个根据DepartmentName属性进行模糊查询的表达式
                if (!param.GatewayTypeName.IsEmpty())
                {
                    expression = expression.And(t => t.GatewayTypeName.Contains(param.GatewayTypeName));
                }
            }
            return expression;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Alfred.Data;
using Alfred.Model.Param.GwManage;
using Alfred.Data.Repository;
using System.Linq;
using Alfred.Model.Param.GwDataManage;
using Alfred.Entity.GwDataManage;
using System.Linq.Expressions;
using Alfred.Util.Extension;

namespace Alfred.Service.GwDataManage
{
    public class GatewayDataService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<GatewayDataEntity>> GetList(GatewayDataParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }
        #endregion

        #region 私有方法
        private Expression<Func<GatewayDataEntity,bool>> ListFilter(GatewayDataParam param)
        {
            var expression = LinqExtensions.True<GatewayDataEntity>();
            if (param != null)
            {
                if (param.DataId != 0)
                {
                    int a = 0;
                }
            }
            return expression;
        }

        #endregion
    }
}

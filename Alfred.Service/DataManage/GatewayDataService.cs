using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Alfred.Data;
using Alfred.Model.Param.GatewayManage;
using Alfred.Data.Repository;
using System.Linq;
using Alfred.Model.Param.GatewayDataManage;
using Alfred.Entity.GatewayDataManage;
using System.Linq.Expressions;
using Alfred.Util.Extension;

namespace Alfred.Service.GatewayDataManage
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
                //Id
                if (param.Id != 0)
                {
                    expression=expression.And(t=>t.GatewayTypeId==param.Id);
                }
                //网关名称
                if (!string.IsNullOrEmpty(param.GatewayName))
                {
                    expression = expression.And(t => t.GatewayName.Contains(param.GatewayName));
                }
                //模板名称
                if (!string.IsNullOrEmpty(param.DataTypeName))
                {
                    expression=expression.And(t=>t.DataTypeName.Contains(param.DataTypeName));
                }
                //开始时间
                if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
                {
                    expression = expression.And(t => t.DataAcqTime >= param.StartTime);
                }
                //结束时间
                if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
                {
                    param.EndTime = param.EndTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    expression = expression.And(t => t.DataAcqTime <= param.EndTime);
                }
            }
            return expression;
        }

        #endregion
    }
}

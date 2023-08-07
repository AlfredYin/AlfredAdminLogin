using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Alfred.Data;
using Alfred.Entity.GwManage;
using Alfred.Model.Param.GwManage;
using Alfred.Data.Repository;
using System.Linq;

namespace Alfred.Service.GwManage
{
    public class GatewayService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<GatewayEntity>> GetList(GatewayParam param)
        {
            var strSql=new StringBuilder();
            List<DbParameter> filter = ListFilter(param,strSql);
            var list= await this.BaseRepository().FindList<GatewayEntity>(strSql.ToString(), filter.ToArray());
            return list.ToList();
        }
        private List<DbParameter> ListFilter(GatewayParam param, StringBuilder strSql)
        {
            strSql.Append(@"SELECT  a.GwId,
                                    a.GwClientId,
                                    a.GwConnectedDate,
                                    a.GwConnectedTltDate,
                                    a.GwConnectingFlag,
                                    a.GwType");
            strSql.Append(@"         FROM    SysGatewayConnected a
                            WHERE   1 = 1");
            var parameter = new List<DbParameter>();
            if (param != null)
            {
                if (param.GwType>=0)
                {
                    //strSql.Append(" AND a.GwType like @GwType");
                    //parameter.Add(DbParameterExtension.CreateDbParameter("@GwType", '%' + param.GwType + '%'));
                }
                //if (param.NewsType > 0)
                //{
                //    strSql.Append(" AND a.NewsType = @NewsType");
                //    parameter.Add(DbParameterExtension.CreateDbParameter("@NewsType", param.NewsType));
                //}
                //if (!string.IsNullOrEmpty(param.NewsTag))
                //{
                //    strSql.Append(" AND a.NewsTag like @NewsTag");
                //    parameter.Add(DbParameterExtension.CreateDbParameter("@NewsTag", '%' + param.NewsTag + '%'));
                //}
                //if (param.ProvinceId > 0)
                //{
                //    strSql.Append(" AND a.ProvinceId = @ProvinceId");
                //    parameter.Add(DbParameterExtension.CreateDbParameter("@ProvinceId", param.ProvinceId));
                //}
                //if (param.CityId > 0)
                //{
                //    strSql.Append(" AND a.CityId = @CityId");
                //    parameter.Add(DbParameterExtension.CreateDbParameter("@CityId", param.CityId));
                //}
                //if (param.CountyId > 0)
                //{
                //    strSql.Append(" AND a.CountId = @CountId");
                //    parameter.Add(DbParameterExtension.CreateDbParameter("@CountyId", param.CountyId));
                //}
            }
            return parameter;
        }
        #endregion
    }
}

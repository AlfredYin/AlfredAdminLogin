using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Business.Cache;
using Alfred.Business.SystemManage;
using Alfred.Cache.Factory;
using Alfred.Entity;
using Alfred.Entity.GwDataManage;
using Alfred.Entity.OrganizationManage;
using Alfred.Entity.SystemManage;
using Alfred.Enum;
using Alfred.Enum.OrganizationManage;
using Alfred.Model;
using Alfred.Model.Param;
using Alfred.Model.Param.OrganizationManage;
using Alfred.Service.GwDataManage;
using Alfred.Service.OrganizationManage;
using Alfred.Util;
using Alfred.Util.Extension;
using Alfred.Util.Model;
using Alfred.Web.Code;
using Alfred.Model.Param.GwDataManage;

namespace Alfred.Business.GwDataManage
{
    public class GatewayDataBLL
    {
        private GatewayDataService gatewayDataService=new GatewayDataService();

        #region 获取数据
        public async Task<TData<List<GatewayDataEntity>>> GetList(GatewayDataParam param)
        {
            TData<List<GatewayDataEntity>> obj = new TData<List<GatewayDataEntity>>();
            obj.Data = await gatewayDataService.GetList(param);
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}

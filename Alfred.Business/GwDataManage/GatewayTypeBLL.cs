using Alfred.Entity.GwDataManage;
using Alfred.Entity.OrganizationManage;
using Alfred.Model.Param.GwDataManage;
using Alfred.Model.Result;
using Alfred.Service.GwDataManage;
using Alfred.Util.Model;
using Alfred.Web.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alfred.Business.GwDataManage
{
    public class GatewayTypeBLL
    {
        private GatewayTypeService gatewayTypeService = new GatewayTypeService();

        #region 获取数据

        public async Task<TData<List<ZtreeInfo>>> GetZtreeGatewayTypeList(GatewayTypeParam param)
        {
            var obj=new TData<List<ZtreeInfo>>();   
            obj.Data = new List<ZtreeInfo>();
            List<GatewayTypeEntity> gatewayTypeList = await gatewayTypeService.GetList(param);
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            if (operatorInfo.IsSystem != 1)
            {
                //递归获取子网关类型
                List<long> childrenGatewayTypeList= new List<long>();
            }
            //对网关类型信息进行处理
            foreach(GatewayTypeEntity gatewayTypeEntity in gatewayTypeList)
            {
                obj.Data.Add(new ZtreeInfo
                {
                    id = gatewayTypeEntity.Id,
                    pId = gatewayTypeEntity.ParentId,
                    name = gatewayTypeEntity.GatewayTypeName
                });
            }
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}

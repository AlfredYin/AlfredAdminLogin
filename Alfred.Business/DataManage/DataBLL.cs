using Alfred.Entity.GatewayDataManage;
using Alfred.Entity.GwDataManage;
using Alfred.Model.Param.GatewayDataManage;
using Alfred.Model.Result;
using Alfred.Service.GatewayDataManage;
using Alfred.Util.Model;
using Alfred.Web.Code;

namespace Alfred.Business.DataManage
{
    public class DataBLL
    {
        private GatewayDataService gatewayDataService = new GatewayDataService();

        private GatewayTypeService gatewayTypeService = new GatewayTypeService();

        #region 获取数据  ---  网关获取数据
        public async Task<TData<List<GatewayDataEntity>>> GetGatewayDataList(GatewayDataParam param)
        {
            TData<List<GatewayDataEntity>> obj = new TData<List<GatewayDataEntity>>();
            obj.Data = await gatewayDataService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 获取数据 --- 网关类型数据 --- ZtreeInfo
        public async Task<TData<List<ZtreeInfo>>> GetGatewayTypeList(GatewayTypeParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();
            List<GatewayTypeEntity> gatewayTypeList = await gatewayTypeService.GetList(param);
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            if (operatorInfo.IsSystem != 1)
            {
                //递归获取子网关类型
                List<long> childrenGatewayTypeList = new List<long>();
            }
            //对网关类型信息进行处理
            foreach (GatewayTypeEntity gatewayTypeEntity in gatewayTypeList)
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

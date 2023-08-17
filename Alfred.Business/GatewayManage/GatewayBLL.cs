using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alfred.Entity.GatewayManage;
using Alfred.Model.Param.GatewayManage;
using Alfred.Service.GatewayManage;
using Alfred.Util.Model;

namespace Alfred.Business.GwManage
{
    public class GatewayBLL
    {
        private GatewayService gatewayService=new GatewayService();

        #region 获取数据
        public async Task<TData<List<GatewayEntity>>> GetList(GatewayParam param)
        {
            TData<List<GatewayEntity>> obj= new TData<List<GatewayEntity>>();

            obj.Data=await gatewayService.GetList(param);
            obj.Total=obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<GatewayEntity>>> GetPageList(GatewayParam param, Pagination pagination)
        {
            TData<List<GatewayEntity>> obj = new TData<List<GatewayEntity>>();

            return obj;
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alfred.Admin.Web.Controllers;
using Alfred.Business.GwManage;
using Alfred.Business.OrganizationManage;
using Alfred.Entity.GwManage;
using Alfred.Model.Param.GwManage;
using Alfred.Util.Model;
using Alfred.Util;
using Alfred.Model.Result;
using Alfred.Service.GwDataManage;
using Alfred.Business.GwDataManage;
using Alfred.Model.Param.GwDataManage;

namespace Alfred.Admin.Web.Areas.GwDataManage.Controllers
{
    [Area("GwDataManage")]
    public class GatewayTypeController : BaseController
    {
        //BLL
        private GatewayTypeBLL gatewayTypeBLL= new GatewayTypeBLL();

        //
        [AuthorizeFilter("gwdata:gatewaydata:view")]
        public IActionResult GatewayRTDataIndex()
        {
            return View();
        }

        //http://localhost:5000/GwDataManage/GatewayType/GatewayDataIndex
        [AuthorizeFilter("gwdata:gatewaydata:view")]
        public IActionResult GatewayDataIndex()
        {
            return View();
        }

        //http://localhost:5000
        //~GwDataManage/GatewayData/GetPageListJson
        //?pageSize=10&pageIndex=1&sort=Id&sortType=desc&DepartmentId=&UserName=&Mobile=&UserStatus=&StartTime=&EndTime=&_=1691132776082
        //获取网关管理中的数据
        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("gwdata:gatewaydata:search")]
        public async Task<IActionResult> GetPageListJson(GatewayTypeParam param)
        {
            return Json(new object[] { });
        }

        //http://localhost:5000/GwDataManage/GatewayData/GetGatewayTypeTreeListJson
        //GetGwTreeListJson  的Json 左侧网关类型展示树
        [HttpGet]
        [AuthorizeFilter("gwdata:gatewaydata:search")]
        public async Task<IActionResult> GetTreeListJson(GatewayTypeParam param)
        {
            TData<List<ZtreeInfo>> obj = await gatewayTypeBLL.GetZtreeGatewayTypeList(param);
            return Json(obj);
        }
        #endregion

    }
}

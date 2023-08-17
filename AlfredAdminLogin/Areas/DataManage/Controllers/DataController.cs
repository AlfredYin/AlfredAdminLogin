using Alfred.Admin.Web.Controllers;
using Alfred.Business.DataManage;
using Alfred.Entity.GatewayDataManage;
using Alfred.Entity.GatewayManage;
using Alfred.Model.Param.GatewayDataManage;
using Alfred.Model.Result;
using Alfred.Util.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlfredAdminLogin.Areas.DataManage.Controllers
{
    [Area("DataManage")]
    public class DataController : Controller
    {
        private DataBLL dataBLL = new DataBLL();

        public IActionResult DataIndex()
        {
            return View();
        }

        #region 获取数据  获取数据列表
        [HttpGet]
        [AuthorizeFilter()]
        public async Task<IActionResult> GetGatewayDataListJson(GatewayDataParam param)
        {
            TData<List<GatewayDataEntity>> obj = await dataBLL.GetGatewayDataList(param);
            return Json(obj);
        }
        #endregion

        #region 获取数据 左侧网关类型展示树
        [HttpGet]
        [AuthorizeFilter()]
        public async Task<IActionResult> GetGatewayTypeTreeListJson(GatewayTypeParam param)
        {
            TData<List<ZtreeInfo>> obj = await dataBLL.GetGatewayTypeList(param);
            return Json(obj);
        }

        #endregion
    }
}

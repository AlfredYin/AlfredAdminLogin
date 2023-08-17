using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alfred.Admin.Web.Controllers;
using Alfred.Business.GwManage;
using Alfred.Business.OrganizationManage;
using Alfred.Entity.GatewayManage;
using Alfred.Model.Param.GatewayManage;
using Alfred.Util.Model;

namespace Alfred.Admin.Web.Areas.GatewayManage.Controllers
{
    [Area("GatewayManage")]
    public class GatewayController : Controller
    {
        private GatewayBLL gatewayBLL= new GatewayBLL();

        [AuthorizeFilter()]
        public IActionResult GatewayIndex()
        {
            return View();
        }

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter()]
        public async Task<IActionResult> GetListJson(GatewayParam param)
        {
            TData<List<GatewayEntity>> obj = await gatewayBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter()]
        public async Task<IActionResult> GetPageListJson(GatewayParam param,Pagination pagination)
        {
            TData<List<GatewayEntity>> obj = await gatewayBLL.GetList(param);
            return Json(obj);
        }
        #endregion
    }
}

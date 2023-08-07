using Alfred.Admin.Web.Controllers;
using Alfred.Business.GwDataManage;
using Alfred.Business.GwManage;
using Alfred.Entity.GwDataManage;
using Alfred.Entity.OrganizationManage;
using Alfred.Model.Param.GwDataManage;
using Alfred.Util.Model;
using Microsoft.AspNetCore.Mvc;

namespace AlfredAdminLogin.Areas.GwDataManage.Controllers
{
    [Area("GwDataManage")]
    public class GatewayDataValueController : Controller
    {
        //BLL
        private GatewayDataBLL gatewayDataBLL= new GatewayDataBLL();
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
        [AuthorizeFilter()]
        public async Task<IActionResult> GetPageListJson(GatewayDataParam param)
        {
            TData<List<GatewayDataEntity>> obj = await gatewayDataBLL.GetList(param);
            return Json(obj);
        }

        #endregion
    }
}

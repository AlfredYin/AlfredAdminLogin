using Alfred.Admin.Web.Controllers;
using Alfred.Business.DeviceManage;
using Alfred.Entity.DeviceManage;
using Alfred.Model.Param.DeviceManage;
using Alfred.Util.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlfredAdminLogin.Areas.DeviceManage.Controllers
{
    [Area("DeviceManage")]
    public class DeviceController : Controller
    {
        private DeviceBll deviceBll= new DeviceBll();

        //设备列表主视图,展示设备列表
        [AuthorizeFilter()]
        public IActionResult DeviceIndex()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeFilter()]
        public async Task<IActionResult> GetListJson(DeviceParam param)
        {
            TData<List<DeviceEntity>> obj= await deviceBll.GetList(param);
            return Json(obj);
        }

        //设别模板主视图,展示模板列表
        [AuthorizeFilter()]
        public IActionResult DeviceTemplateIndex()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeFilter()]
        public async Task<IActionResult> GetTemplateListJson(DeviceTemplateParam param)
        {
            TData<List<DeviceTeplateEntity>> obj = await deviceBll.GetTemplateList(param);
            return Json(obj);
        }
    }
}

using Alfred.Admin.Web.Controllers;
using Alfred.Business.CalElectChargesManage;
using Alfred.Entity.CalElectChargesManage;
using Alfred.Model.Param;
using Alfred.Util;
using Alfred.Util.Model;
using Microsoft.AspNetCore.Mvc;

namespace AlfredAdminLogin.Areas.CalElectChargesManage.Controllers
{
    [Area("CalElectChargesManage")]
    public class CalElectChargesController : Controller
    {
        private string charges = "";

        private CalElectChargesBLL calElectChargesBLL = new CalElectChargesBLL();

        /// <summary>
        /// CalElectCharges的Index视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter()]
        public IActionResult CalElectChargesIndex()
        {
            return View();
        }

        /// <summary>
        /// 转到导入的视图,CalElectCharges的Index视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter()]
        public IActionResult ElectImport()
        {
            return View();
        }

        /// <summary>
        /// 处理上传的Excel并解析
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter()]
        public async Task<IActionResult> ImportElectJson(ImportParam param)
        {
            //对excel进行处理
            List<ElectEntity> list = new ExcelHelper<ElectEntity>().ImportFromExcel(param.FilePath);

            //对列表进行处理
            TData obj = calElectChargesBLL.CalElectCharges(list);

            charges = obj.Message;

            return Json(obj);
        }

        //http://localhost:5000/CalElectChargesManage/CalElectCharges/CalElectChargesJQueryIndex
        /// <summary>
        /// CalElectChargesJQuery的Index视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter()]
        public IActionResult CalElectChargesJQueryIndex()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeFilter()]
        public IActionResult CalElectChargesJQueryCharges()
        {
            return Json(charges);
        }
    }
}

using Alfred.Admin.Web.Controllers;
using Alfred.Business.CalElectChargesManage;
using Alfred.Entity.CalElectChargesManage;
using Alfred.Model.Param;
using Alfred.Model.Param.ElectChargesManage;
using Alfred.Util;
using Alfred.Util.Model;
using Microsoft.AspNetCore.Mvc;

namespace AlfredAdminLogin.Areas.CalElectChargesManage.Controllers
{
    [Area("ElectChargesManage")]
    public class ElectChargesController : Controller
    {
        private string charges = "";

        private ElectChargesBLL electChargesBLL = new ElectChargesBLL();

        #region   每日电量电费展示首页 获取数据

        [HttpGet]
        [AuthorizeFilter()]
        public IActionResult ElectChargesIndex()
        {
            return View();
        }

        //获取逐条的每日电量
        //http://localhost:5000/ElectChargesManage/ElectCharges/GetListJson?pageSize=10&pageIndex=1&sort=Id&sortType=desc&DepartmentId=&StartTime=2023-08-08&EndTime=2023-08-09&_=1691569219152
        [HttpGet]
        [AuthorizeFilter()]
        public async Task<IActionResult> GetListJson(ElectChargesParam param)
        {
            TData<List<ElectChargesEntity>> obj = await electChargesBLL.GetList(param);
            return Json(obj);
        }

        //带页数的请求
        //http://localhost:5000/ElectChargesManage/ElectCharges/GetListJson?pageSize=10&pageIndex=1&sort=Id&sortType=desc&DepartmentId=&StartTime=2023-08-08&EndTime=2023-08-09&_=1691569219152

        #endregion

        #region 合计电量的统计 获取数据

        [HttpGet]
        [AuthorizeFilter()]
        public IActionResult ElectChargesSumIndex()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeFilter()]
        public async Task<IActionResult> GetSumListJson(ElectChargesParam param)
        {
            TData<List<ElectChargesEntity>> obj = await electChargesBLL.GetSumList(param);
            return Json(obj);
        } 

        #endregion

        #region 手动导入电量部分

        /// <summary>
        /// CalElectCharges的Index视图 --- 手动导入
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
            List<CountEntity> list = new ExcelHelper<CountEntity>().ImportFromExcel(param.FilePath);

            //对列表进行处理
            TData obj = electChargesBLL.CalElectCharges(list);

            charges = obj.Message;

            return Json(obj);
        }


        #endregion

        #region JQuery 导入电量计算部分
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

        #endregion
    }
}

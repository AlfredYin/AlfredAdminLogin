using Alfred.Admin.Web.Controllers;
using Alfred.Business.CalElectChargesManage;
using Alfred.Entity.CalElectChargesManage;
using Alfred.Model.Param;
using Alfred.Model.Param.ElectChargesManage;
using Alfred.Util;
using Alfred.Util.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        /// <summary>
        /// CalElectCharges的Index视图 --- 手动导入 ----------------- 这个首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter()]
        public IActionResult CalElectChargesIndex()
        {
            return View();
        }

        #region 手动导入电量部分 主视图

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

        #endregion

        #region 导入24 小时数据部分  ElectImport form表单 和 解析Excel导入数据库数据部分   ---------  暂时不导入数据库,因为不知道 怎么从小时

        /// <summary>
        /// 处理上传的Excel并解析 24各个小时电量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter()]
        public async Task<IActionResult> ImportElectJson(ImportParam param)
        {

            //使用Serivce进行处理

            //对excel进行处理
            List<CountElectEntity> hourElectList = new ExcelHelper<CountElectEntity>().ImportFromExcel(param.FilePath);

            List<ElectChargesEntity> segmentList = new List<ElectChargesEntity>();
            //hourElectList 将 转换成为  SegmentList
            segmentList = await electChargesBLL.HoursToSegment(hourElectList);

            //此处是数据存入数据库
            TData obj = await electChargesBLL.ImportElectBySegment(segmentList);

            return Json(obj);
        }

        #endregion

        #region 导入峰尖峰谷低谷四段电量模板  ElectImportBySegment form表单 和 解析Excel导入数据库数据 BySegment部分

        /// <summary>
        /// 转到导入的视图,CalElectCharges的Index视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter()]
        public IActionResult ElectImportBySegment()
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
        public async Task<IActionResult> ImportElectBySegmentJson(ImportParam param)
        {

            //使用Serivce进行处理

            //对excel进行处理
            List<ElectChargesEntity> list = new ExcelHelper<ElectChargesEntity>().ImportFromExcel(param.FilePath);

            //对列表进行处理
            //TData obj = electChargesBLL.CalElectCharges(list);

            //此处是数据存入数据库
            TData obj = await electChargesBLL.ImportElectBySegment(list);

            return Json(obj);
        }

        #endregion

        #region JQuery 导入电量计算部分  ------------ 已经弃用,等以后再学前端更改这一部分
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

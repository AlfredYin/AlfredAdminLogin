using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Alfred.Admin.Web.Controllers;
using Alfred.Business.OrganizationManage;
using Alfred.Entity.OrganizationManage;
using Alfred.Model;
using Alfred.Model.Param.OrganizationManage;
using Alfred.Model.Result;
using Alfred.Util.Model;

namespace Alfred.Admin.Web.Areas.OrganizationManage.Controllers
{
    [Area("OrganizationManage")]
    public class DepartmentController : BaseController
    {
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        #region 视图功能

        //部门管理
        //DepartmentIndex
        [AuthorizeFilter("organization:department:view")]
        public IActionResult DepartmentIndex()
        {
            return View();
        }

        //DepartmentForm
        public IActionResult DepartmentForm()
        {
            return View();
        }
        #endregion

        //获取部门管理中的数据
        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:department:search,organization:user:search")]
        public async Task<IActionResult> GetListJson(DepartmentListParam param)
        {
            TData<List<DepartmentEntity>> obj = await departmentBLL.GetList(param);
            return Json(obj);
        }

        //http://localhost:5000/OrganizationManage/Department/GetDepartmentTreeListJson
        [HttpGet]
        [AuthorizeFilter("organization:department:search,organization:user:search")]        //GetDepartmentTreeListJson 的Json 左侧单位组织中的员工管理
        public async Task<IActionResult> GetDepartmentTreeListJson(DepartmentListParam param)
        {
            TData<List<ZtreeInfo>> obj = await departmentBLL.GetZtreeDepartmentList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:department:view")]
        public async Task<IActionResult> GetUserTreeListJson(DepartmentListParam param)
        {
            TData<List<ZtreeInfo>> obj = await departmentBLL.GetZtreeUserList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:department:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<DepartmentEntity> obj = await departmentBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            TData<int> obj = await departmentBLL.GetMaxSort();
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:department:add,organization:department:edit")]
        public async Task<IActionResult> SaveFormJson(DepartmentEntity entity)
        {
            TData<string> obj = await departmentBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:department:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await departmentBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
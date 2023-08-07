using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Alfred.Admin.Web.Controllers;
using Alfred.Business.OrganizationManage;
using Alfred.Business.SystemManage;
using Alfred.Entity.OrganizationManage;
using Alfred.Model.Param;
using Alfred.Model.Param.OrganizationManage;
using Alfred.Model.Result;
using Alfred.Model.Result.SystemManage;
using Alfred.Util;
using Alfred.Util.Model;
using Alfred.Web.Code;

namespace Alfred.Admin.Web.Areas.OrganizationManage.Controllers
{
    [Area("OrganizationManage")]
    public class UserController : BaseController
    {
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        #region 视图功能
        //员工管理 http://localhost:5000/OrganizationManage/User/UserIndex Get请求
        //转到Index
        [AuthorizeFilter("organization:user:view")]
        public IActionResult UserIndex()
        {
            return View();  //User控制器下的UserIndex视图
        }

        public IActionResult UserForm()
        {
            return View();
        }

        public IActionResult UserDetail()
        {
            ViewBag.Ip = NetHelper.Ip;
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public async Task<IActionResult> ChangePassword()
        {
            ViewBag.OperatorInfo = await Operator.Instance.Current();
            return View();
        }

        public IActionResult ChangeUser()
        {
            return View();
        }

        public async Task<IActionResult> UserPortrait()
        {
            ViewBag.OperatorInfo = await Operator.Instance.Current();
            return View();
        }

        public IActionResult UserImport()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:user:search")]
        public async Task<IActionResult> GetListJson(UserListParam param)
        {
            TData<List<UserEntity>> obj = await userBLL.GetList(param);
            return Json(obj);
        }
        /*
         * public class UserListParam : DateTimeParam
        {
        public string UserName { get; set; }

        public string Mobile { get; set; }

        public int? UserStatus { get; set; }

        public long? DepartmentId { get; set; }

        public List<long> ChildrenDepartmentIdList { get; set; }

        public string UserIds { get; set; }
        }
         */
        /*
         * Pagination
         * 分页参数
         * 
         */
        //http://localhost:5000/OrganizationManage/User/GetPageListJson
        //?pageSize=10&pageIndex=1&sort=Id&sortType=desc&DepartmentId=&UserName=&Mobile=&UserStatus=&StartTime=&EndTime=&_=1690867437901
        //获取右侧的用户信息的列表
        [HttpGet]
        [AuthorizeFilter("organization:user:search")]
        public async Task<IActionResult> GetPageListJson(UserListParam param, Pagination pagination)    //获取列表的Json
        {
            //获取TData<List<UserEntity>>格式的 用户信息列表,并以json的格式返回
            TData<List<UserEntity>> obj = await userBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:user:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<UserEntity> obj = await userBLL.GetEntity(id);
            return Json(obj);
        }

        //获取用户的授权菜单的Json结构体
        [HttpGet]
        [AuthorizeFilter("organization:user:view")]
        public async Task<IActionResult> GetUserAuthorizeJson() //获取用户信息授权的Json格式数据
        {
            TData<UserAuthorizeInfo> obj = new TData<UserAuthorizeInfo>();
            //操作人员的信息"一沙软件""管理员"
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            //获取授权的菜单
            //接下来，通过调用MenuAuthorizeBLL类的GetAuthorizeList方法获取授权的菜单，并将结果保存在一个TData<List<MenuAuthorizeInfo>>对象中
            TData<List<MenuAuthorizeInfo>> objMenuAuthorizeInfo = await new MenuAuthorizeBLL().GetAuthorizeList(operatorInfo);
            obj.Data = new UserAuthorizeInfo();
            obj.Data.IsSystem = operatorInfo.IsSystem;
            if (objMenuAuthorizeInfo.Tag == 1)
            {
                obj.Data.MenuAuthorize = objMenuAuthorizeInfo.Data;
            }
            obj.Tag = 1;

            //返回菜单详情
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:user:add,organization:user:edit")]
        public async Task<IActionResult> SaveFormJson(UserEntity entity)
        {
            TData<string> obj = await userBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:user:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await userBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:user:resetpassword")]
        public async Task<IActionResult> ResetPasswordJson(UserEntity entity)
        {
            TData<long> obj = await userBLL.ResetPassword(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:user:edit")]
        public async Task<IActionResult> ChangePasswordJson(ChangePasswordParam entity)
        {
            TData<long> obj = await userBLL.ChangePassword(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:user:edit")]
        public async Task<IActionResult> ChangeUserJson(UserEntity entity)
        {
            TData<long> obj = await userBLL.ChangeUser(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:user:edit")]
        public async Task<IActionResult> ImportUserJson(ImportParam param)
        {
            List<UserEntity> list = new ExcelHelper<UserEntity>().ImportFromExcel(param.FilePath);
            TData obj = await userBLL.ImportUser(param, list);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:user:edit")]
        public async Task<IActionResult> ExportUserJson(UserListParam param)
        {
            TData<string> obj = new TData<string>();
            TData<List<UserEntity>> userObj = await userBLL.GetList(param);
            if (userObj.Tag == 1)
            {
                string file = new ExcelHelper<UserEntity>().ExportToExcel("用户列表.xls",
                                                                          "用户列表",
                                                                          userObj.Data,
                                                                          new string[] { "UserName", "RealName", "Gender", "Mobile", "Email" });
                obj.Data = file;
                obj.Tag = 1;
            }
            return Json(obj);
        }
        #endregion
    }
}
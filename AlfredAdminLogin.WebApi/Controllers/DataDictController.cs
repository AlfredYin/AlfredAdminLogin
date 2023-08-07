using Alfred.Admin.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Alfred.Business.SystemManage;
using System.Threading.Tasks;
using Alfred.Util.Model;
using System.Collections.Generic;
using Alfred.Model.Result.SystemManage;
using Alfred.Model.Param.SystemManage;

namespace AlfredAdminLogin.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeFilter]
    public class DataDictController : ControllerBase
    {
        private DataDictBLL dataDictBLL = new DataDictBLL();

        #region 获取数据
        [HttpGet]
        public async Task<TData<List<DataDictInfo>>> GetList([FromQuery]DataDictListParam param)
        {
            TData<List<DataDictInfo>> obj = await dataDictBLL.GetDataDictList();
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}

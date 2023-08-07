using Alfred.Admin.WebApi.Controllers;
using Alfred.Business.OrganizationManage;
using Alfred.Entity.OrganizationManage;
using Alfred.Model.Param;
using Alfred.Model.Param.OrganizationManage;
using Alfred.Util.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlfredAdminLogin.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeFilter]
    public class NewsController : ControllerBase
    {
        private NewsBLL newsBLL= new NewsBLL();

        #region 获取数据

        [HttpGet]
        public async Task<TData<List<NewsEntity>>> GetPageList([FromQuery] NewsListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<NewsEntity>> obj=await newsBLL.GetPageList(param, pagination);
            return obj;
        }

        [HttpGet]
        public async Task<TData<List<NewsEntity>>> GetPageContentList([FromQuery] NewsListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<NewsEntity>> obj=await newsBLL.GetPageContentList(param, pagination);
            return obj;
        }

        [HttpGet]
        public async Task<TData<NewsEntity>> GetForm([FromQuery]long id)
        {
            TData<NewsEntity> obj=await newsBLL.GetEntity(id);
            return obj;
        }
        #endregion

        #region 提交数据
        [HttpPost]
        public async Task<TData<string>> SaveForm([FromBody]NewsEntity entity)
        {
            TData<string> obj=await newsBLL.SaveForm(entity);
            return obj;
        }

        [HttpPost]
        public async Task<TData<string>> SaveViewTimes([FromBody]IdParam param)
        {
            TData<string> obj = null;
            TData<NewsEntity> objNews = await newsBLL.GetEntity(param.Id.Value);
            NewsEntity newsEntity= new NewsEntity();    

            if(objNews.Data!=null)
            {
                newsEntity.Id= param.Id.Value;
                newsEntity.ViewTimes=objNews.Data.ViewTimes;
                newsEntity.ViewTimes++;
                obj = await newsBLL.SaveForm(newsEntity);
            }
            else
            {
                obj = new TData<string>();
                obj.Message = "文章不存在";
            }
            return obj;
        }
        #endregion
    }
}

﻿using Alfred.Admin.WebApi.Controllers;
using Alfred.Util;
using Alfred.Util.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlfredAdminLogin.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeFilter]
    public class FileController : ControllerBase
    {
        #region 上传单个文件
        [HttpPost]
        public async Task<TData<string>> UploadFile(int fileModule,IFormCollection fileList)
        {
            TData<string> obj = await FileHelper.UploadFile(fileModule, fileList.Files);
            return obj;
        }
        #endregion

        #region 删除单个文件
        [HttpPost]
        public TData<string> DeleteFile(int fileModule , string filePath)
        {
            TData<string> obj=FileHelper.DeleteFile(fileModule, filePath);
            return obj;
        }
        #endregion

        #region 下载文件
        [HttpGet]
        public FileContentResult DownloadFile (string filePath,int delete = 1)
        {
            TData<FileContentResult> obj=FileHelper.DownloadFile(filePath, delete);
            if (obj.Tag == 1)
            {
                return obj.Data;
            }
            else
            {
                throw new System.Exception("下载失败 : " + obj.Message);
            }
        }
        #endregion
    }
}

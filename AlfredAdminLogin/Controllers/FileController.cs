﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Alfred.Util;
using Alfred.Util.Extension;
using Alfred.Util.Model;

namespace Alfred.Admin.Web.Controllers
{
    public class FileController : BaseController
    {
        #region 上传单个文件
        [HttpPost]
        [AuthorizeFilter]
        public async Task<TData<string>> UploadFile(int fileModule, IFormCollection fileList)
        {
            TData<string> obj = await FileHelper.UploadFile(fileModule, fileList.Files);
            return obj;
        }
        #endregion

        #region 删除单个文件
        [HttpPost]
        [AuthorizeFilter]
        public TData<string> DeleteFile(int fileModule, string filePath)
        {
            TData<string> obj = FileHelper.DeleteFile(fileModule, filePath);
            return obj;
        }
        #endregion

        #region 下载文件
        [HttpGet]
        public FileContentResult DownloadFile(string filePath, int delete = 1)
        {
            TData<FileContentResult> obj = FileHelper.DownloadFile(filePath, delete);
            if (obj.Tag == 1)
            {
                return obj.Data;
            }
            else
            {
                throw new Exception("下载失败：" + obj.Message);
            }
        }
        #endregion
    }
}
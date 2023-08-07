using System;
using System.Collections.Generic;
using Alfred.Model.Param.SystemManage;

namespace Alfred.Model.Param.OrganizationManage
{
    public class NewsListParam : BaseAreaParam
    {
        /*
    public class BaseAreaParam : BaseApiToken
    {
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? CountyId { get; set; }
        /// <summary>
        /// 逗号分隔的Id
        /// </summary>
        public string AreaId { get; set; }
    }
        */
        public string NewsTitle { get; set; }
        public int? NewsType { get; set; }
        public string NewsTag { get; set; }
    }
}

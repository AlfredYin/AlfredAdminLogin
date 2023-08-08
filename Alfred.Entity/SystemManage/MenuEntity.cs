using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Util;

namespace Alfred.Entity.SystemManage
{
    //实体,实体通常被用于建模领域。例如，在一个电商网站的后台系统中，商品、用户、订单等可以被看作是实体。
    //每个实体都有自己的属性（例如商品名称、价格、用户ID等）
    //和
    //行为（例如用户下单、商品上架等）。
    //菜单实体
    [Table("SysMenu")]
    public class MenuEntity : BaseExtensionEntity
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ParentId { get; set; }

        public string MenuName { get; set; }

        public string MenuIcon { get; set; }

        public string MenuUrl { get; set; }

        public string MenuTarget { get; set; }

        public int? MenuSort { get; set; }

        public int? MenuType { get; set; }

        public int? MenuStatus { get; set; }
        public string Authorize { get; set; }

        public string Remark { get; set; }

        [NotMapped]
        public string ParentName { get; set; }
    }
}

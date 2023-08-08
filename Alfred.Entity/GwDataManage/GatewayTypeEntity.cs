using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Util;

namespace Alfred.Entity.GwDataManage
{
    [Table("SysGatewayType")]
    public class GatewayTypeEntity : BaseExtensionEntity
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ParentId { get; set; }
        public string GatewayTypeName { get; set; }
        public int TypeNo { get; set; }
        public string str1 { get; set; }
        public string str2 { get; set; }
        public string str3 { get; set; }
        public long? PrincipalId { get; set; }
        public int? GatewayTypeSort { get; set; }

        [NotMapped]
        public string GatewayExplain { get; set; } 
        [NotMapped]
        public string GatewayExplain1 { get; set;}
    }
}

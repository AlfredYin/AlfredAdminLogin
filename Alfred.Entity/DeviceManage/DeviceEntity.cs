using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Entity.DeviceManage
{
    [Table("DeviceList")]
    public class DeviceEntity : BaseExtensionEntity
    {
        public string DeviceName { get; set; }
        public int DeviceTemplateId { get; set; }
        public int GatewayId { get; set; }
        [NotMapped]
        public string DeviceTemplateName { get; set; }
        [NotMapped]
        public string GatewayName { get; set; }
        public string? str1 { get; set; }
        public string? str2 { get; set; }
        public string? str3 { get; set; }

        [NotMapped]
        public string? BelongOrganization { get; set; }
        public string? DeviceTitle { get; set; }
        public int DeviceConnectingFlag { get; set; }
    }
}

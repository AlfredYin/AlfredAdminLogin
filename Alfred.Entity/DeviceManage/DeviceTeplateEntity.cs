using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Entity.DeviceManage
{
    [Table("DeviceTemplateList")]
    public class DeviceTeplateEntity : BaseExtensionEntity
    {
        public string DeviceTemplateName { get; set; }
        public int DevicesCount { get; set; }
        public int VarCount { get; set; }
        public string? str1 { get; set; }
        public string? str2 { get; set; }
        public string? str3 { get; set; }
        [NotMapped]
        public string? BelongOrganization { get; set; }
        [NotMapped]
        public string? GetType { get; set; }
        public string? DeviceTemplateTitle { get; set; }
    }
}

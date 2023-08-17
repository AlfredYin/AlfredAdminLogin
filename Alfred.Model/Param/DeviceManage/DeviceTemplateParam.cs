using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Model.Param.DeviceManage
{
    public class DeviceTemplateParam : DateTimeParam
    {
        public long Id { get; set; }
        public string DeviceTemplateName { get; set; }
        public string DeviceTemplateTitle { get; set;}
    }
}

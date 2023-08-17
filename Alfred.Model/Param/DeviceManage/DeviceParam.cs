using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Model.Param.DeviceManage
{
    public class DeviceParam : DateTimeParam
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public int DeviceTemplateId { get; set; }
        public int GatewayId { get; set; }
        public string DeviceTitle { get; set;}
    }
}

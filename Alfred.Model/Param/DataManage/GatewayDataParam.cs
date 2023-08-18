using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Model.Param.GatewayDataManage
{
    public class GatewayDataParam : DateTimeParam
    {
        public int Id { get; set; }
        public string GatewayName { get; set; }
        public string DataTypeName { get; set; }
        //public DateTime DataAcqTime { get; set; }
        //public int SlaveId { get; set; }
    }
}

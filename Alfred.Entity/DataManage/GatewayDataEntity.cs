using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Alfred.Util;

namespace Alfred.Entity.GatewayDataManage
{
    [Table("GatewayData_All")]
    public class GatewayDataEntity : BaseEntity
    {
        //使用Id代替DataID
        //public int DataId { get; set; }
        public string GatewayName { get; set; }
        public int GatewayId { get; set; }
        public int DataTypeId { get; set; }
        public string DataTypeName { get; set; }    
        public string DataValue { get; set; }
        public DateTime DataAcqTime { get; set; }
        public string DataUnit { get; set; }    
        public int GatewayTypeId { get; set; }
        public int SlaveId { get; set; }
    }
}

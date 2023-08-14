using Alfred.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Entity.CalElectChargesManage
{
    [Table("SysElectCharge")]
    public class ElectChargesEntity :  BaseExtensionEntity
    {
        [Description("尖峰")]
        public double spike_value { get; set; }

        [Description("波峰")]
        public double peak_value { get; set; }

        [Description("波谷")]
        public double valley_value { get; set; }

        [Description("低谷")]
        public double bottom_value { get; set; }   
        
        public double spike_price { get; set; }
        public double peak_price { get; set; }
        public double valley_price { get; set; }
        public double bottom_price { get; set; }
        public int LoopId { get; set; }

        //选择最大容量
        [NotMapped]
        public int ElectType { get; set; }  // 如果是1 则为最大容量计费 如果2 则为最大需量计费
        //变压器 容量
        [NotMapped]
        public int ContentKVA { get; set; }
        [NotMapped]
        public double sum_charges { get; set; }         //总电费

        //结果由系统自己计算出来
        [NotMapped]
        public double spike_charges { get; set; }
        [NotMapped]
        public double peak_charges { get; set; }
        [NotMapped]
        public double valley_charges { get; set; }
        [NotMapped]
        public double bottom_charges { get; set; }

        [Description("回路名称")]
        public string LoopName { get; set;}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Entity.CalElectChargesManage
{
    [Table("SysElectCharge")]
    public class ElectChargesEntity :  BaseExtensionEntity
    {
        public long Id { get; set; }
        public double spike_value { get; set; }
        public double peak_value { get; set; }
        public double valley_value { get; set; }
        public double bottom_value { get; set; }    
        public double spike_price { get; set; }
        public double peak_price { get; set; }
        public double valley_price { get; set; }
        public double bottom_price { get; set; }
        public int LoopId { get; set; } 

        //结果由系统自己计算出来
        [NotMapped]
        public double spike_charges { get; set; }
        [NotMapped]
        public double peak_charges { get; set; }
        [NotMapped]
        public double valley_charges { get; set; }
        [NotMapped]
        public double bottom_charges { get; set; }
        public string LoopName { get; set;}
    }
}

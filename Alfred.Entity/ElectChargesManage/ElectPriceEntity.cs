using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Entity.ElectChargesManage
{
    [Table("SysElectPrice")]
    public class ElectPriceEntity: BaseExtensionEntity
    {
        public double spike_price { get; set; }
        public double peak_price { get; set; }
        public double valley_price { get; set; }
        public double bottom_price { get; set; }
        public double normal_price { get; set; }
        public string Province { get; set; }    
    }
}

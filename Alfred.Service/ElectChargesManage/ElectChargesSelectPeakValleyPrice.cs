using Alfred.Entity.CalElectChargesManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Service.ElectChargesManage
{
    public class ElectChargesSelectPeakValleyPrice
    {
        public ElectChargesEntity SelectPeakValleyPrice(ElectChargesEntity entity,string province)
        {
            //这里应当使用数据库中的山东电价
            if (province=="shangdong")
            {
                entity.spike_price = 1.2863;
                entity.peak_price = 0.8998;
                entity.valley_price = 0.3658;
                entity.bottom_price = 0.3460;
                entity.normal_price = 0.7244;

                //entity.spike_price = 1;
                //entity.peak_price = 1;
                //entity.valley_price = 1;
                //entity.bottom_price = 1;
                //entity.normal_price = 1;
            }
            
            //其它省份
            //不同的电费价格

            return entity;
        }

        //判断回路暂时加到这里
        public ElectChargesEntity SelectLoopId(ElectChargesEntity entity)
        {
            if (entity.LoopName == "一号回路")
            {
                entity.LoopId = 1;
            }
            if (entity.LoopName == "二号回路")
            {
                entity.LoopId = 2;
            }
            if (entity.LoopName == "三号回路")
            {
                entity.LoopId = 3;
            }
            else
            {
                entity.LoopId = -1;
            }
            return entity;
        }
    }
}

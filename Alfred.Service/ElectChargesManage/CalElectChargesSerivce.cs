using Alfred.Entity.CalElectChargesManage;
using Alfred.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Service.ElectChargesManage
{
    public class CalElectChargesSerivce
    {
        //电费价格
        private double elect_price = 0.5520;

        //每日电费和
        private double day_elect_sum = 0.0;
        //总电费和
        private double total_elect_sum = 0.0;

        /// <summary>
        /// 计算List的总电费
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public TData CalElectCharges(List<CountElectEntity> list)
        {
            TData obj = new TData();
            return obj;
        }

        /// <summary>
        /// 私有函数,计算每小时的电费
        /// </summary>
        /// <param name="_electricity"></param>
        /// <param name="month"></param>
        /// <param name="_hours"></param>
        /// <returns></returns>
        private double GetCalResultByOneHour(double _electricity, int month, int _hours)
        {
            
            return 0.0;
        }

        //将小时电量转换成为 时段电量
        public async Task<List<ElectChargesEntity>> HoursToSegment(List<CountElectEntity> countElectEntities)
        {
            List<ElectChargesEntity> electChargesEntities = new List<ElectChargesEntity>();


            //开始转换
            int month;

            foreach(var countElectEntity in countElectEntities)
            {
                DateTime? dateTimeNullable = countElectEntity.BaseCreateTime;
                
                month = dateTimeNullable.HasValue ? dateTimeNullable.Value.Month : 0;

                ElectChargesEntity electChargesEntity=new ElectChargesEntity();
                //时间
                electChargesEntity.BaseCreateTime = countElectEntity.BaseCreateTime;
                //回路名称
                electChargesEntity.LoopName = countElectEntity.LoopName;
                //冬季
                if (month == 1 || month == 12)
                {
                    //谷时段
                    electChargesEntity.valley_value = countElectEntity.time_10 + countElectEntity.time_11 + countElectEntity.time_15 + countElectEntity.time_16;
                    //深谷时段
                    electChargesEntity.bottom_value = countElectEntity.time_12 + countElectEntity.time_13 + countElectEntity.time_14;
                    //峰时段
                    electChargesEntity.peak_value = countElectEntity.time_20 + countElectEntity.time_21 + countElectEntity.time_22;
                    //尖峰时段
                    electChargesEntity.spike_value = countElectEntity.time_16 + countElectEntity.time_17 + countElectEntity.time_18 + countElectEntity.time_19;
                    //平时段
                    electChargesEntity.normal_value = 
                        countElectEntity.time_23 + countElectEntity.time_00 + countElectEntity.time_01 
                        + countElectEntity.time_02 + countElectEntity.time_03 + countElectEntity.time_04 
                        + countElectEntity.time_05 + countElectEntity.time_06 + countElectEntity.time_07 
                        + countElectEntity.time_08 + countElectEntity.time_09;
                }
                //春季
                if (month == 2 || month == 3 || month == 4 || month == 5)
                {
                    //谷时段
                    electChargesEntity.valley_value = countElectEntity.time_10 + countElectEntity.time_15 ;
                    //深谷时段
                    electChargesEntity.bottom_value = countElectEntity.time_11 + countElectEntity.time_12 + countElectEntity.time_13 + countElectEntity.time_14;
                    //峰时段
                    electChargesEntity.peak_value = countElectEntity.time_17 + countElectEntity.time_21 + countElectEntity.time_22;
                    //尖峰时段
                    electChargesEntity.spike_value =  countElectEntity.time_18 + countElectEntity.time_19 + countElectEntity.time_20;
                    //平时段
                    electChargesEntity.normal_value =
                        countElectEntity.time_23 + countElectEntity.time_00 + countElectEntity.time_01
                        + countElectEntity.time_02 + countElectEntity.time_03 + countElectEntity.time_04
                        + countElectEntity.time_05 + countElectEntity.time_06 + countElectEntity.time_07
                        + countElectEntity.time_08 + countElectEntity.time_09 + countElectEntity.time_16;

                }
                //夏季
                if (month == 6 || month == 7 || month == 8)
                {
                    //谷时段
                    electChargesEntity.valley_value = countElectEntity.time_02 + countElectEntity.time_03 + countElectEntity.time_04
                        + countElectEntity.time_05 + countElectEntity.time_06 + countElectEntity.time_07
                        + countElectEntity.time_08;
                    //深谷时段 --- 夏季不存在深谷
                    //electChargesEntity.bottom_value = countElectEntity.time_11+countElectEntity.time_12 + countElectEntity.time_13 + countElectEntity.time_14;
                    //峰时段
                    electChargesEntity.peak_value = countElectEntity.time_16 + countElectEntity.time_17;
                    //尖峰时段
                    electChargesEntity.spike_value = countElectEntity.time_18 + countElectEntity.time_19 + countElectEntity.time_20 + countElectEntity.time_21 + countElectEntity.time_22;
                    //平时段
                    electChargesEntity.normal_value =
                        countElectEntity.time_23 + countElectEntity.time_00 + countElectEntity.time_01
                        + countElectEntity.time_09 + countElectEntity.time_10 + countElectEntity.time_11 
                        + countElectEntity.time_12 + countElectEntity.time_13 + countElectEntity.time_14 + countElectEntity.time_15;
                }
                //秋季
                if (month == 9 || month == 10 || month == 11)
                {
                    //谷时段
                    electChargesEntity.valley_value = countElectEntity.time_10 + countElectEntity.time_15;
                    //深谷时段
                    electChargesEntity.bottom_value = countElectEntity.time_11 + countElectEntity.time_12 + countElectEntity.time_13 + countElectEntity.time_14;
                    //峰时段
                    electChargesEntity.peak_value = countElectEntity.time_16 + countElectEntity.time_20 + countElectEntity.time_21;
                    //尖峰时段
                    electChargesEntity.spike_value = countElectEntity.time_17 + countElectEntity.time_18 + countElectEntity.time_19;
                    //平时段
                    electChargesEntity.normal_value =
                        countElectEntity.time_23 + countElectEntity.time_00 + countElectEntity.time_01
                        + countElectEntity.time_02 + countElectEntity.time_03 + countElectEntity.time_04
                        + countElectEntity.time_05 + countElectEntity.time_06 + countElectEntity.time_07
                        + countElectEntity.time_08 + countElectEntity.time_09 + countElectEntity.time_22;
                }

                electChargesEntities.Add(electChargesEntity);
            }

            return electChargesEntities;
        }
    }
}

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
            if (list.Any())
            {
                foreach (CountElectEntity e in list)
                {
                    day_elect_sum = 0.0;
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_00, e.Month, 0);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_01, e.Month, 1);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_02, e.Month, 2);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_03, e.Month, 3);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_04, e.Month, 4);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_05, e.Month, 5);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_06, e.Month, 6);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_07, e.Month, 7);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_08, e.Month, 8);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_09, e.Month, 9);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_10, e.Month, 10);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_11, e.Month, 11);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_12, e.Month, 12);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_13, e.Month, 13);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_14, e.Month, 14);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_15, e.Month, 15);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_16, e.Month, 16);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_17, e.Month, 17);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_18, e.Month, 18);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_19, e.Month, 19);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_20, e.Month, 20);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_21, e.Month, 21);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_22, e.Month, 22);
                    day_elect_sum = day_elect_sum + GetCalResultByOneHour(e.time_23, e.Month, 23);
                    total_elect_sum = total_elect_sum + day_elect_sum;
                }
                obj.Message = total_elect_sum.ToString();
                obj.Tag = 1;
            }
            else
            {
                obj.Message = "未找到导入的数据";
            }

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
            if (month == 0)
            {
                return 0;
            }
            if (elect_price == 0)
            {
                return 0;
            }
            //冬季
            if (month == 1 || month == 12)
            {
                //谷时段
                if ((_hours >= 10 && _hours < 12) || (_hours >= 14 && _hours < 16))
                {
                    return _electricity * 1.0 * elect_price * 0.3;
                }
                //深谷时段
                if (_hours >= 12 && _hours < 14)
                {
                    return _electricity * 1.0 * elect_price * 0.1;
                }
                //峰时段
                if (_hours >= 19 && _hours < 22)
                {
                    return _electricity * 1.0 * elect_price * 1.7;
                }
                //尖峰时段
                if (_hours >= 16 && _hours < 19)
                {
                    return _electricity * 1.0 * elect_price * 2.0;
                }
                //平时段
                return _electricity * 1.0 * elect_price * 1.0;
            }
            //春季
            if (month == 2 || month == 3 || month == 4 || month == 5)
            {
                //谷时段
                if ((_hours >= 10 && _hours < 11) || (_hours >= 14 && _hours < 15))
                {
                    return _electricity * 1.0 * elect_price * 0.3;
                }
                //深谷时段
                if (_hours >= 11 && _hours < 14)
                {
                    return _electricity * 1.0 * elect_price * 0.1;
                }
                //峰时段
                if ((_hours >= 17 && _hours < 18) || (_hours >= 20 && _hours < 22))
                {
                    return _electricity * 1.0 * elect_price * 1.7;
                }
                //尖峰时段
                if (_hours >= 18 && _hours < 20)
                {
                    return _electricity * 1.0 * elect_price * 2.0;
                }
                //平时段
                return _electricity * 1.0 * elect_price * 1.0;
            }
            //夏季
            if (month == 6 || month == 7 || month == 8)
            {
                //谷时段
                if (_hours >= 2 && _hours < 8)
                {
                    return _electricity * 1.0 * elect_price * 0.3;
                }
                //峰时段
                if (_hours >= 16 && _hours < 18)
                {
                    return _electricity * 1.0 * elect_price * 1.7;
                }
                //尖峰时段
                if (_hours >= 18 && _hours < 22)
                {
                    return _electricity * 1.0 * elect_price * 2.0;
                }
                //平时段
                return _electricity * 1.0 * elect_price * 1.0;
            }

            //秋季
            if (month == 9 || month == 10 || month == 11)
            {
                //谷时段
                if ((_hours >= 10 && _hours < 11) || (_hours >= 14 && _hours < 15))
                {
                    return _electricity * 1.0 * elect_price * 0.3;
                }
                //深谷时段
                if (_hours >= 11 && _hours < 14)
                {
                    return _electricity * 1.0 * elect_price * 0.1;
                }
                //峰时段
                if ((_hours >= 16 && _hours < 17) || (_hours >= 19 && _hours < 21))
                {
                    return _electricity * 1.0 * elect_price * 1.7;
                }
                //尖峰时段
                if (_hours >= 17 && _hours < 19)
                {
                    return _electricity * 1.0 * elect_price * 2.0;
                }
                //平时段
                return _electricity * 1.0 * elect_price * 1.0;
            }
            return 0.0;
        }
    }
}

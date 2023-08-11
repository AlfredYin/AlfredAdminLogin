using Alfred.Data.Repository;
using Alfred.Entity.CalElectChargesManage;
using Alfred.Model.Param.ElectChargesManage;
using Alfred.Util.Extension;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Service.ElectChargesManage
{
    public class ElectChargesSerivce : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<ElectChargesEntity>> GetList(ElectChargesParam param)
        {
            //添加判断条件
            var expression = ListFilter(param);

            //通过条件获取数据
            var list = await this.BaseRepository().FindList(expression);

            return CalChargesByList(list.ToList());   
        }

        //展示使用异步,
        //此处暂留
        //获取用户分页列表取消
        public async Task<List<ElectChargesEntity>> GetSumList(ElectChargesParam param) 
        {
            var expression=ListFilter(param);
            var list = (await this.BaseRepository().FindList(expression)).ToList();
            //继续使用ElectChargesEntity的实体
            List<ElectChargesEntity> sumList = (from item in list
                                                     group item by new
                                                     {
                                                         item.LoopName,
                                                         item.spike_price,
                                                         item.peak_price,
                                                         item.valley_price,
                                                         item.bottom_price,
                                                         item.LoopId,
                                                     } into grp
                                                     select new ElectChargesEntity
                                                     {
                                                         LoopName = grp.Key.LoopName,
                                                         spike_value = grp.Sum(x => x.spike_value),
                                                         spike_price = grp.Key.spike_price,
                                                         peak_value = grp.Sum(x => x.peak_value),
                                                         peak_price = grp.Key.peak_price,
                                                         valley_value = grp.Sum(x => x.valley_value),
                                                         valley_price = grp.Key.valley_price,
                                                         bottom_value = grp.Sum(x => x.bottom_value),
                                                         bottom_price = grp.Key.bottom_price,
                                                         LoopId=grp.Key.LoopId
                                                     }).ToList();

            //肯定要改写成为Linq语句的
            //Linq 语句 GroupBy
            //返回returnlist
            return CalChargesByList(sumList.ToList());
        }

        #endregion

        #region 通过数据库获取的电量和电费列表 私有方法
        public List<ElectChargesEntity> CalChargesByList(List<ElectChargesEntity> list)
        {
            foreach(var row in list)
            {
                row.spike_charges = row.spike_price * Convert.ToDouble(row.spike_value);
                row.peak_charges=row.peak_price*Convert.ToDouble(row.peak_value);
                row.valley_charges = row.valley_price * Convert.ToDouble(row.valley_value);
                row.bottom_charges = row.bottom_price * Convert.ToDouble(row.bottom_value);
            }
            return list;
        }

        #endregion

        #region 参数填充的私有方法
        private Expression<Func<ElectChargesEntity,bool>> ListFilter(ElectChargesParam param)
        {
            var expression = LinqExtensions.True<ElectChargesEntity>();
            if(param != null)
            {
                //搜索访问加上LoopName 回路名称
                if (!string.IsNullOrEmpty(param.LoopName))
                {
                    expression=expression.And(t=>t.LoopName.Contains(param.LoopName));
                }
                //开始时间
                if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
                {
                    expression = expression.And(t => t.BaseCreateTime >= param.StartTime);
                }
                //结束时间
                if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
                {
                    expression = expression.And(t => t.BaseCreateTime <= param.EndTime);
                }
            }

            return expression;
        }
        #endregion
    }
}

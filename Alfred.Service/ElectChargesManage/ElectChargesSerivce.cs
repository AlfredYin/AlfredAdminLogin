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
using Alfred.Util;
using Alfred.IdGenerator;

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
                                                         item.normal_price,
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
                                                         normal_value=grp.Sum(x => x.normal_value),
                                                         normal_price=grp.Key.normal_price,
                                                         LoopId=grp.Key.LoopId
                                                     }).ToList();

            //肯定要改写成为Linq语句的
            //Linq 语句 GroupBy

            if (param.ContentKVA != 0)
            {
                ElectChargesEntity sumItem = new ElectChargesEntity()
                {
                    spike_price = sumList.Last().spike_price,
                    peak_price = sumList.Last().peak_price,
                    valley_price = sumList.Last().valley_price,
                    bottom_price = sumList.Last().bottom_price,
                    normal_price= sumList.Last().normal_price,
                };
                sumItem.LoopName = "回路合并";
                sumItem.LoopId = -1;

                double spikeSum = 0.0;
                double peakSum = 0.0;
                double valleySum = 0.0;
                double bottomSum = 0.0;
                double normalSum=0.0;

                foreach (var item in sumList)
                {
                    spikeSum = item.spike_value + spikeSum;
                    peakSum = item.peak_value + peakSum;
                    valleySum = item.valley_value + valleySum;
                    bottomSum = item.bottom_value + bottomSum;
                    normalSum= item.normal_value + normalSum;
                }

                sumItem.spike_value = spikeSum;
                sumItem.peak_value = peakSum;
                sumItem.valley_value = valleySum;
                sumItem.bottom_value = bottomSum;
                sumItem.normal_value = normalSum;

                if (param.ElectType == 1)
                {
                    //山东
                    sumItem.sum_charges = param.ContentKVA * 28;
                }
                if(param.ElectType == 2)
                {
                    sumItem.sum_charges = param.ContentKVA * 38;
                }

                sumList.Add(sumItem);
            }
            //返回returnlist
            return CalChargesByList(sumList.ToList());
        }

        #endregion

        #region 通过数据库获取的电量和电费列表 私有方法
        public List<ElectChargesEntity> CalChargesByList(List<ElectChargesEntity> list)
        {

            

            foreach(var row in list)
            {
                double sum = 0.0;
                //保留四位小数
                row.spike_charges = Math.Round(row.spike_price * Convert.ToDouble(row.spike_value), 4);
                row.peak_charges= Math.Round(row.peak_price * Convert.ToDouble(row.peak_value), 4);
                row.valley_charges = Math.Round(row.valley_price * Convert.ToDouble(row.valley_value), 4);
                row.bottom_charges = Math.Round(row.bottom_price * Convert.ToDouble(row.bottom_value), 4);
                row.normal_charges = Math.Round(row.normal_price * Convert.ToDouble(row.normal_value), 4);
                sum = sum + row.spike_charges+ row.peak_charges + row.valley_charges +row.bottom_charges + row.normal_charges;

                row.sum_charges= row.sum_charges+sum;
                row.sum_charges=Math.Round(row.sum_charges,4);
            }
            return list;
        }

        #endregion

        #region 将数据存入数据库
        public async Task SaveForm(ElectChargesEntity entity)
        {
            var db = await this.BaseRepository().BeginTrans();
            try
            {
                //主键 怎么拼凑 ??? 新闻 this.Id = IdGeneratorHelper.Instance.GetId();
                entity.Id=IdGeneratorHelper.Instance.GetId();

                //填充entity中必填的数据
                entity.BaseIsDelete = 0;
                entity.BaseCreatorId = 1;
                entity.BaseModifierId= 1;
                entity.BaseVersion= 1;

                //省份是山东
                ElectPriceSerive electChargesSelectPeakValleyPrice=new ElectPriceSerive();
                entity=await electChargesSelectPeakValleyPrice.SelectPeakValleyPrice(entity,"山东");

                //回路Id
                entity = electChargesSelectPeakValleyPrice.SelectLoopId(entity);



                //不修改创建时间
                //只更改修改时间
                //await entity.Modify();
                entity.BaseModifyTime = DateTime.Now;
                await db.Insert(entity);

                await db.CommitTrans();
            }
            catch (Exception ex)
            {
                await db.RollbackTrans();
                throw;
            }
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
                //BaseCreateTime 即 StartTime
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

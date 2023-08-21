using Alfred.Data.Repository;
using Alfred.Entity.CalElectChargesManage;
using Alfred.Entity.ElectChargesManage;
using Alfred.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Service.ElectChargesManage
{
    public class ElectPriceSerive: RepositoryFactory
    {
        public async Task<ElectChargesEntity> SelectPeakValleyPrice(ElectChargesEntity entity,string province)
        {
            ElectPriceEntity dbPriceEntity=new ElectPriceEntity();
            bool getFlag = false;
            //这里应当使用数据库中的山东电价
            if (province=="山东" && getFlag==false)
            {
                try
                {
                    var expression = LinqExtensions.True<ElectPriceEntity>();
                    expression = expression.And(t => t.Province == province);
                    ElectPriceEntity priceEntity = await this.BaseRepository().FindEntity<ElectPriceEntity>(expression);
                    dbPriceEntity.spike_price = priceEntity.spike_price;
                    dbPriceEntity.peak_price = priceEntity.peak_price;
                    dbPriceEntity.valley_price = priceEntity.valley_price;
                    dbPriceEntity.bottom_price = priceEntity.bottom_price;
                    dbPriceEntity.normal_price = priceEntity.normal_price;

                    entity.spike_price = dbPriceEntity.spike_price;
                    entity.peak_price = dbPriceEntity.peak_price;
                    entity.valley_price = dbPriceEntity.valley_price;
                    entity.bottom_price = dbPriceEntity.bottom_price;
                    entity.normal_price = dbPriceEntity.normal_price;

                    getFlag =true;
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
            else
            {
                entity.spike_price = dbPriceEntity.spike_price;
                entity.peak_price = dbPriceEntity.peak_price;
                entity.valley_price = dbPriceEntity.valley_price;
                entity.bottom_price = dbPriceEntity.bottom_price;
                entity.normal_price = dbPriceEntity.normal_price;
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
        
        //获取电费列表
        public async Task<List<ElectPriceEntity>> GetPriceList()
        {
            var expression = LinqExtensions.True<ElectPriceEntity>();
            expression = expression.And(t => t.Province == "山东");
            var priceEntities = await this.BaseRepository().FindList(expression);

            return priceEntities.ToList();
        }

        public Task SaveForm(ElectPriceEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

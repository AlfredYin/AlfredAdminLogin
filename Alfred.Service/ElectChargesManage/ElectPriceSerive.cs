using Alfred.Data.Repository;
using Alfred.Entity.CalElectChargesManage;
using Alfred.Entity.ElectChargesManage;
using Alfred.Entity.OrganizationManage;
using Alfred.Util;
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
            //expression = expression.And(t => t.Province == "山东");
            var priceEntities = await this.BaseRepository().FindList(expression);

            return priceEntities.ToList();
        }

        //保存表单
        public async Task SaveForm(ElectPriceEntity entity)
        {
            //if (entity.Id.IsNullOrZero())
            //{
                long? id = entity.Id;
                await entity.Create();
                entity.Id = id;
                await this.BaseRepository().Insert<ElectPriceEntity>(entity);   
            //}
            //else
            //{
            //    try
            //    {
            //        await entity.Modify();
            //        await this.BaseRepository().Update<ElectPriceEntity>(entity);
            //    }
            //   catch(Exception ex)
            //    {
            //        ;
            //    }
            //}
        }

        //更改表单
        public async Task UpdateForm(ElectPriceEntity entity)
        {
            long? id = entity.Id;
            await entity.Modify();
            await this.BaseRepository().Update<ElectPriceEntity>(entity);
        }

        //判断是否存在用户名(省份)
        public bool ExistProvince(ElectPriceEntity entity)
        {
            var expression = LinqExtensions.True<ElectPriceEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression=expression.And(t=>t.Province== entity.Province); 
            }
            else
            {
                expression=expression.And(t=>t.Province== entity.Province && t.Id == entity.Id);
            }

            int a = this.BaseRepository().IQueryable(expression).Count();

            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }

    }
}

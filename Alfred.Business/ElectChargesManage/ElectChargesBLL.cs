using Alfred.Entity.CalElectChargesManage;
using Alfred.Entity.ElectChargesManage;
using Alfred.Entity.OrganizationManage;
using Alfred.Model.Param;
using Alfred.Model.Param.ElectChargesManage;
using Alfred.Service.ElectChargesManage;
using Alfred.Service.OrganizationManage;
using Alfred.Util.Extension;
using Alfred.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Business.CalElectChargesManage
{
    public class ElectChargesBLL
    {
        private CalElectChargesSerivce calElectChargesSerivce = new CalElectChargesSerivce();
        private ElectChargesSerivce electChargesSerivce=new ElectChargesSerivce();
        private ElectPriceSerive electPriceSerive=new ElectPriceSerive();

        #region 获取数据库历史电量电费

        public async Task<TData<List<ElectChargesEntity>>> GetList(ElectChargesParam param)
        {
            TData<List<ElectChargesEntity>> obj=new TData<List<ElectChargesEntity>>();
            obj.Data = await electChargesSerivce.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        //带分页的
        //
        //

        //删除历史数据
        public async Task<TData> DeleteAll()
        {
            TData obj = new TData();
            await electChargesSerivce.DeleteAll();
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 获取历史电费之和数据
        public async Task<TData<List<ElectChargesEntity>>> GetSumList(ElectChargesParam param)
        {
            TData<List<ElectChargesEntity>> obj = new TData<List<ElectChargesEntity>>();
            obj.Data = await electChargesSerivce.GetSumList(param);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 手动导入 每小时电量数据,并计算总电费

        /// <summary>
        /// 传入电费实体列表,计算总电费
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public TData CalElectCharges(List<CountElectEntity> list)
        {
            TData data = new TData();
            data=calElectChargesSerivce.CalElectCharges(list);
            return data;
        }

        /// <summary>
        /// 将Excel文件读出来的数据存入
        /// </summary>
        /// <returns></returns>
        public async Task<TData> ImportElect()
        {
            TData obj = new TData();


            return obj;
        }

        #endregion

        #region 手动导入 每段电量数据,并计算总电费

        /// <summary>
        /// 传入电费实体列表,计算总电费
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //public TData CalElectChargesBySegment(List<ElectChargesEntity> list)
        //{
        //    TData data = new TData();
        //    data = calElectChargesSerivce.CalElectChargesBySegment(list);
        //    return data;
        //}
        public async Task<TData> ImportElectBySegment(List<ElectChargesEntity> list)
        {
            TData obj = new TData();

            if (list.Any())
            {
                foreach(ElectChargesEntity e in list)
                {
                    //数据入库格式  ------------- 经典   先获取 再替换,再导入
                    //ElectChargesEntity dbEntity = await electChargesSerivce.GetEntity();
                    ElectChargesEntity dbEntity = null;

                    if (dbEntity != null)
                    {
                        //处理重复数据,一般没有直接忽略
                    }
                    else
                    {
                        await electChargesSerivce.SaveForm(e);
                    }
                }
                obj.Message = "导入成功";
                obj.Tag=1;
            }
            else
            {
                obj.Message = "未找到导入的数据";
            }

            return obj;
        }

        #endregion

        #region 获取电价信息

        public async Task<TData<List<ElectPriceEntity>>> GetPriceList()
        {
            TData<List<ElectPriceEntity>> obj=new TData<List<ElectPriceEntity>>();
            obj.Data = await electPriceSerive.GetPriceList(); 
            obj.Tag= 1;
            return obj;
        }

        //提交Form电价 加入
        public async Task<TData<string>> SaveForm(ElectPriceEntity entity)
        {
            TData<string> obj = new TData<string>();

            if (electPriceSerive.ExistProvince(entity))
            {
                await electPriceSerive.UpdateForm(entity);
                obj.Data = entity.Id.ParseToString();
                obj.Tag = 1;
                return obj;
            }

            await electPriceSerive.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        //更新修改
        //public async Task<TData> UpdatePrice(ElectPriceEntity entity)
        //{
        //    TData obj= new TData();
        //    await electPriceSerive.SaveForm(entity);

        //    obj.Tag = 1;
        //    return obj;
        //}
        #endregion

        public async Task<List<ElectChargesEntity>> HoursToSegment(List<CountElectEntity> countElectEntities)
        {
            List<ElectChargesEntity> electChargesEntities = new List<ElectChargesEntity>();

            electChargesEntities = await calElectChargesSerivce.HoursToSegment(countElectEntities);


            return electChargesEntities;
        }
    }
}

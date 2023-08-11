using Alfred.Entity.CalElectChargesManage;
using Alfred.Model.Param.ElectChargesManage;
using Alfred.Service.ElectChargesManage;
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

        #endregion

        #region
        public async Task<TData<List<ElectChargesEntity>>> GetSumList(ElectChargesParam param)
        {
            TData<List<ElectChargesEntity>> obj = new TData<List<ElectChargesEntity>>();
            obj.Data = await electChargesSerivce.GetSumList(param);
            obj.Tag = 1;
            return obj;
        }

        #endregion


        #region 手动导入电量数据,并计算总电费

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

        public async Task<TData> ImportElect()
        {
            TData obj = new TData();


            return obj;
        }

        #endregion
    }
}

using Alfred.Entity.DeviceManage;
using Alfred.Model.Param.DeviceManage;
using Alfred.Service.DeviceManage;
using Alfred.Util.Model;

namespace Alfred.Business.DeviceManage
{
    public class DeviceBll
    {
        private DeviceService   deviceService   =new DeviceService();
        private DeviceTemplateService deviceTemplateService=new DeviceTemplateService();

        #region 获取数据    设别列表
        //获取设备列表
        public async Task<TData<List<DeviceEntity>>> GetList(DeviceParam param)
        {
            TData<List<DeviceEntity>> obj= new TData<List<DeviceEntity>>();
            obj.Data = await deviceService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 获取数据      模板列表
        //获取模板列表
        public async Task<TData<List<DeviceTeplateEntity>>> GetTemplateList(DeviceTemplateParam param)
        {
            TData<List<DeviceTeplateEntity>> obj = new TData<List<DeviceTeplateEntity>>();
            //obj.Data = deviceTemplateService.GetList();
            obj.Data = await deviceTemplateService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        //

        #endregion
    }
}

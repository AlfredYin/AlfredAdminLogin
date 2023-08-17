using Alfred.Data.Repository;
using Alfred.Entity.DeviceManage;
using Alfred.Entity.OrganizationManage;
using Alfred.Model.Param.DeviceManage;
using Alfred.Util;
using Alfred.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Service.DeviceManage
{
    public class DeviceService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<DeviceEntity>> GetList(DeviceParam param)
        {
            var expression = ListFilter(param);

            var list = await this.BaseRepository().FindList(expression);

            var returnList= list.ToList();

            return AddListVar(returnList);
        }
        #endregion

        #region 私有方法,填充参数

        private Expression<Func<DeviceEntity,bool>> ListFilter(DeviceParam param)
        {
            var expression = LinqExtensions.True<DeviceEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.DeviceName))
                {
                    expression = expression.And(t => t.DeviceName.Contains(param.DeviceName));
                }
                //是否具有DeviceTemplateId参数
                if (param.DeviceTemplateId!=0)
                {
                    expression = expression.And(t => t.DeviceTemplateId==param.DeviceTemplateId);
                }
                //状况
                if (param.GatewayId !=0)
                {
                    expression = expression.And(t => t.GatewayId == param.GatewayId);
                }
                //标签
                if (!string.IsNullOrEmpty(param.DeviceTitle))
                {
                    expression = expression.And(t => t.DeviceTitle.Contains(param.DeviceTitle));
                }
            }
            return expression;
        }
        #endregion

        #region 私有方法,普通设备列表中的设备模板和所属网关
        List<DeviceEntity> AddListVar(List<DeviceEntity> deviceEntities)
        {
            foreach (var item in deviceEntities)
            {
                //这个通过数据库获取
                if (item.DeviceTemplateId == 100001)
                {
                    item.BelongOrganization = "根组织";
                    item.DeviceTemplateName = "HF600型模板";
                    item.GatewayName = "user_m100_0";
                }
                if(item.DeviceTemplateId == 100002)
                {
                    item.BelongOrganization = "根组织";
                    item.DeviceTemplateName = "BA72型模板";
                    item.GatewayName = "user_m100_0";
                }
            }
            return deviceEntities;
        }

        #endregion
    }
}

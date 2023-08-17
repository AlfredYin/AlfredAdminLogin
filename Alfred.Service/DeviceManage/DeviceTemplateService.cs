using Alfred.Data.Repository;
using Alfred.Entity.DeviceManage;
using Alfred.Model.Param.DeviceManage;
using Alfred.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Service.DeviceManage
{
    public class DeviceTemplateService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<DeviceTeplateEntity>> GetList(DeviceTemplateParam param)
        {
            var expression = ListFilter(param);

            var list = await this.BaseRepository().FindList(expression);

            var returnList=list.ToList();

            //填充所属组织和采集方式

            return AddListVar(returnList);
        }
        #endregion

        #region 私有方法,填充参数

        private Expression<Func<DeviceTeplateEntity, bool>> ListFilter(DeviceTemplateParam param)
        {
            var expression = LinqExtensions.True<DeviceTeplateEntity>();
            if (param != null)
            {
                //模板名字
                if (!string.IsNullOrEmpty(param.DeviceTemplateName))
                {
                    expression = expression.And(t => t.DeviceTemplateName.Contains(param.DeviceTemplateName));
                }
                //标签
                if (!string.IsNullOrEmpty(param.DeviceTemplateTitle))
                {
                    expression = expression.And(t => t.DeviceTemplateTitle.Contains(param.DeviceTemplateTitle));
                }
            }
            return expression;
        }
        #endregion

        #region 私有方法,普通模板里所属组织和采集方式

        List<DeviceTeplateEntity> AddListVar(List<DeviceTeplateEntity> deviceTeplateEntities)
        {
            foreach (var item in deviceTeplateEntities)
            {
                item.BelongOrganization = "根组织";
                item.GetType = "云端轮询";
            }
            return deviceTeplateEntities;
        }

        #endregion

    }
}

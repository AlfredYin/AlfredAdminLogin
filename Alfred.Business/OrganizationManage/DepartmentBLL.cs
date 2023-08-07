using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alfred.Entity.OrganizationManage;
using Alfred.Enum.OrganizationManage;
using Alfred.Model;
using Alfred.Model.Result;
using Alfred.Model.Param.OrganizationManage;
using Alfred.Service.OrganizationManage;
using Alfred.Util;
using Alfred.Util.Model;
using Alfred.Util.Extension;
using Alfred.Web.Code;

namespace Alfred.Business.OrganizationManage
{
    public class DepartmentBLL
    {
        private DepartmentService departmentService = new DepartmentService();
        private UserService userService = new UserService();

        #region 获取数据
        //参数 --- 部门 --- Ids DepartmentName
        //这个方法的作用是获取部门列表，并根据当前操作员的权限和部门信息进行筛选和处理，最后返回处理后的部门列表。
        //异步方法调用 使用await关键字等待departmentService.GetList方法的返回结果，并将返回结果赋值给obj.Data.
        public async Task<TData<List<DepartmentEntity>>> GetList(DepartmentListParam param)
        {
            //返回的TData<List<DepartmentEntity>> obj对象
            TData<List<DepartmentEntity>> obj = new TData<List<DepartmentEntity>>();

            //Ids DepartmentName
            obj.Data = await departmentService.GetList(param);

            //接着，使用await关键字等待Operator.Instance.Current方法的返回结果，并将返回结果赋值给operatorInfo变量
            OperatorInfo operatorInfo = await Operator.Instance.Current();

            //判断是不是系统管理员
            if (operatorInfo.IsSystem != 1)
            {
                //如果不是系统管理员，就调用GetChildrenDepartmentIdList方法获取当前操作员所属部门以及子部门的ID列表，
                //并将返回结果赋值给childrenDepartmentIdList变量。
                //这段代码使用了异步方法GetChildrenDepartmentIdList，它的作用是获取指定部门的所有子部门的ID列表。
                //它接受两个参数，第一个参数是部门实体对象obj.Data，第二个参数是当前操作员的部门ID。
                //然后，使用await关键字等待异步方法的执行结果，将返回的子部门ID列表赋值给变量childrenDepartmentIdList。
                //接下来，使用LINQ表达式对obj.Data进行筛选，保留ID在childrenDepartmentIdList中的部门实体对象。具体实现是通过使用Contains方法判断ID是否在childrenDepartmentIdList中。
                //最后，将筛选后的结果转换为列表并重新赋值给obj.Data。
                List<long> childrenDepartmentIdList = await GetChildrenDepartmentIdList(obj.Data, operatorInfo.DepartmentId.Value);

                //Where是LINQ查询表达式中的一个操作符，用于根据指定的条件筛选序列中的元素
                //具体实现是通过Lambda表达式p => childrenDepartmentIdList.Contains(p.Id.Value)来定义筛选条件。Lambda表达式中的变量p表示序列中的每个元素，通过p.Id.Value可以获取到元素的ID值。
                //在Lambda表达式的右侧使用了Contains方法来判断ID值是否存在于childrenDepartmentIdList列表中。如果存在，则保留该元素；如果不存在，则过滤掉该元素。
                //最后，通过调用ToList方法将筛选后的结果转换为一个新的列表，并将其重新赋值给obj.Data。这样就完成了根据子部门ID列表对obj.Data中的元素进行筛选和替换的操作。
                obj.Data = obj.Data.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            }
            
            //用户实体列表
            List<UserEntity> userList = await userService.GetList(new UserListParam { UserIds = string.Join(",", obj.Data.Select(p => p.PrincipalId).ToArray()) });

            //进行筛选
            foreach (DepartmentEntity entity in obj.Data)
            {
                if (entity.PrincipalId > 0)
                {
                    entity.PrincipalName = userList.Where(p => p.Id == entity.PrincipalId).Select(p => p.RealName).FirstOrDefault();
                }
                else
                {
                    entity.PrincipalName = string.Empty;
                }
            }
            obj.Tag = 1;
            return obj;
        }

        //返回左侧的单位组织
        public async Task<TData<List<ZtreeInfo>>> GetZtreeDepartmentList(DepartmentListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();
            List<DepartmentEntity> departmentList = await departmentService.GetList(param);
            OperatorInfo operatorInfo = await Operator.Instance.Current();      //获取操作人员的信息
            if (operatorInfo.IsSystem != 1)
            {
                List<long> childrenDepartmentIdList = await GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId.Value);   //递归获取子机构
                departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            }
            foreach (DepartmentEntity department in departmentList)         //对部门信息进行处理
            {
                obj.Data.Add(new ZtreeInfo
                {
                    id = department.Id,
                    pId = department.ParentId,
                    name = department.DepartmentName
                });
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeUserList(DepartmentListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();
            List<DepartmentEntity> departmentList = await departmentService.GetList(param);
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            if (operatorInfo.IsSystem != 1)
            {
                List<long> childrenDepartmentIdList = await GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId.Value);
                departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            }
            List<UserEntity> userList = await userService.GetList(null);
            foreach (DepartmentEntity department in departmentList)
            {
                obj.Data.Add(new ZtreeInfo
                {
                    id = department.Id,
                    pId = department.ParentId,
                    name = department.DepartmentName
                });
                List<long> userIdList = userList.Where(t => t.DepartmentId == department.Id).Select(t => t.Id.Value).ToList();
                foreach (UserEntity user in userList.Where(t => userIdList.Contains(t.Id.Value)))
                {
                    obj.Data.Add(new ZtreeInfo
                    {
                        id = user.Id,
                        pId = department.Id,
                        name = user.RealName
                    });
                }
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DepartmentEntity>> GetEntity(long id)
        {
            TData<DepartmentEntity> obj = new TData<DepartmentEntity>();
            obj.Data = await departmentService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Data = await departmentService.GetMaxSort();
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DepartmentEntity entity)
        {
            TData<string> obj = new TData<string>();
            if (!entity.Id.IsNullOrZero() && entity.Id == entity.ParentId)
            {
                obj.Message = "不能选择自己作为上级部门！";
                return obj;
            }
            if (departmentService.ExistDepartmentName(entity))
            {
                obj.Message = "部门名称已经存在！";
                return obj;
            }
            await departmentService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            foreach (long id in TextHelper.SplitToArray<long>(ids, ','))
            {
                if (departmentService.ExistChildrenDepartment(id))
                {
                    obj.Message = "该部门下面有子部门！";
                    return obj;
                }
            }
            await departmentService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取当前部门及下面所有的部门
        /// </summary>
        /// <param name="departmentList"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        // 递归获取子部门
        public async Task<List<long>> GetChildrenDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId)
        {
            //递归终止条件
            if (departmentList == null)
            {
                departmentList = await departmentService.GetList(null);
            }
            List<long> departmentIdList = new List<long>();
            departmentIdList.Add(departmentId);
            GetChildrenDepartmentIdList(departmentList, departmentId, departmentIdList);
            return departmentIdList;
        }
        #endregion 

        #region 私有方法
        /// <summary>
        /// 获取该部门下面所有的子部门
        /// </summary>
        /// <param name="departmentList"></param>
        /// <param name="departmentId"></param>
        /// <param name="departmentIdList"></param>
        private void GetChildrenDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId, List<long> departmentIdList)
        {
            var children = departmentList.Where(p => p.ParentId == departmentId).Select(p => p.Id.Value).ToList();
            if (children.Count > 0)
            {
                departmentIdList.AddRange(children);
                foreach (long id in children)
                {
                    //递归获取
                    GetChildrenDepartmentIdList(departmentList, id, departmentIdList);
                }
            }
        }
        #endregion
    }
}

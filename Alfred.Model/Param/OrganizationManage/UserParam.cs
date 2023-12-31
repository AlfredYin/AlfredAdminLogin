﻿using System;
using System.Collections.Generic;

namespace Alfred.Model.Param.OrganizationManage
{
    public class UserListParam : DateTimeParam
    {
        public string UserName { get; set; }

        public string Mobile { get; set; }

        public int? UserStatus { get; set; }

        public long? DepartmentId { get; set; } // 这个属性干啥的???

        public List<long> ChildrenDepartmentIdList { get; set; }

        public string UserIds { get; set; }
    }

    public class ChangePasswordParam
    {
        public long? Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }

}

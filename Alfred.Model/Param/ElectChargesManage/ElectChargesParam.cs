using Alfred.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Model.Param.ElectChargesManage
{
    public class ElectChargesParam : DateTimeParam
    {
        //public string a { get; set; }       //不知道写什么参数,暂时保留a

        public string LoopName { get; set; }
        public int ElectType { get; set; }      //计算类型
        public int ContentKVA { get; set; }      //变压器容量
    }
}

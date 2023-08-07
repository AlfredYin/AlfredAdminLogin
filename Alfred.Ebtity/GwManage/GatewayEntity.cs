using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Alfred.Util;

namespace Alfred.Entity.GwManage
{
    [Table("SysGatewayConnected")]
    public class GatewayEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int GwId { get; set; }
        /// <summary>
        /// Gw Mqtt ClientId
        /// </summary>
        public string GwClientId { get; set; }
        /// <summary>
        /// 第一次开始连接的时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? GwConnectedDate { get; set; }
        /// <summary>
        /// 最后一次连接的时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? GwConnectedTltDate { get; set; }
        /// <summary>
        /// 是否在连接状态
        /// </summary>
        public int GwConnectingFlag { get; set; }
        /// <summary>
        /// 网关的类型
        /// </summary>
        public int GwType { get; set; }
    }
}

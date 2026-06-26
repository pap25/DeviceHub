using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Lis.Dto
{
    public class GetInstrument
    {
        /// <summary>
        /// 仪器ID
        /// </summary>
        public int InstrumentId { get; set; }

        /// <summary>
        /// 仪器型号
        /// </summary>
        public string InstrumentModel { get; set; }

        /// <summary>
        /// 仪器名
        /// </summary>
        public string InstrumentName { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long ExpireTime { get; set; }

        /// <summary>
        /// 授权码状态
        /// </summary>
        public AuthCodeStatus status { get; set; }

        public enum AuthCodeStatus : byte
        {
            /// <summary>
            /// 停用
            /// </summary>
            Disabled = 0,

            /// <summary>
            /// 正常
            /// </summary>
            Normal = 1,
        }
    }
}

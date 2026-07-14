using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Template.Constant
{
    public static class HL7Protocols
    {
        /// <summary>
        /// STX (Start of Text)
        /// 开始文本，ASCII 0x02
        /// ASTM 协议报文开始标识
        /// </summary>
        public const byte STX = 0x02;

        /// <summary>
        /// ETX (End of Text)
        /// 结束文本，ASCII 0x03
        /// ASTM 协议报文结束标识
        /// </summary>
        public const byte ETX = 0x03;

        /// <summary>
        /// EOT (End of Transmission)
        /// 结束传输，ASCII 0x04
        /// ASTM 协议通信结束标识
        /// </summary>
        public const byte EOT = 0x04;

        /// <summary>
        /// ENQ (Enquiry)
        /// 请求发送，ASCII 0x05
        /// ASTM 协议发送请求
        /// </summary>
        public const byte ENQ = 0x05;

        /// <summary>
        /// ACK (Acknowledge)
        /// 确认应答，ASCII 0x06
        /// ASTM 协议确认收到数据
        /// </summary>
        public const byte ACK = 0x06;

        /// <summary>
        /// NAK (Negative Acknowledge)
        /// 否认应答，ASCII 0x15
        /// ASTM 协议表示数据接收失败
        /// </summary>
        public const byte NAK = 0x15;


        /// <summary>
        /// VT (Vertical Tab)
        /// 垂直制表符，ASCII 0x0B
        /// HL7 MLLP 报文开始标识（SB）
        /// </summary>
        public const byte VT = 0x0B;

        /// <summary>
        /// EB (End Block)
        /// 结束块标识，ASCII 0x1C
        /// HL7 MLLP 报文结束标识（EB）
        /// </summary>
        public const byte EB = 0x1C;

        /// <summary>
        /// CR (Carriage Return)
        /// 回车符，ASCII 0x0D
        /// HL7 MLLP 报文结束符
        /// </summary>
        public const byte CR = 0x0D;
    }
}

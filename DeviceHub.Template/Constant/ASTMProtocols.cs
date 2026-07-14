using System;

namespace DeviceHub.Template.Constant
{
    /// <summary>
    /// ASTM 协议控制字符定义
    /// </summary>
    public static class ASTMProtocols
    {
        /// <summary>
        /// 收到 NAK 后重发 ENQ 的等待时间（秒）
        /// </summary>
        public const int NakRetryDelaySeconds = 10;

        /// <summary>
        /// ENQ (Enquiry)
        /// 请求建立通信，ASCII 0x05
        /// 发送方请求与接收方建立会话
        /// </summary>
        public const byte ENQ = 0x05;

        /// <summary>
        /// ACK (Acknowledge)
        /// 确认应答，ASCII 0x06
        /// 表示成功接收数据
        /// </summary>
        public const byte ACK = 0x06;

        /// <summary>
        /// EOT (End Of Transmission)
        /// 结束传输，ASCII 0x04
        /// 表示本次通信结束
        /// </summary>
        public const byte EOT = 0x04;

        /// <summary>
        /// NAK (Negative Acknowledge)
        /// 否认应答，ASCII 0x15
        /// 表示数据接收失败，需要重发
        /// </summary>
        public const byte NAK = 0x15;

        /// <summary>
        /// STX (Start Of Text)
        /// 开始文本，ASCII 0x02
        /// ASTM 数据帧开始标识
        /// </summary>
        public const byte STX = 0x02;

        /// <summary>
        /// LF (Line Feed)
        /// 换行符，ASCII 0x0A
        /// ASTM 记录结束标识之一
        /// </summary>
        public const byte LF = 0x0A;

        /// <summary>
        /// CR (Carriage Return)
        /// 回车符，ASCII 0x0D
        /// ASTM 记录结束标识之一
        /// </summary>
        public const byte CR = 0x0D;

        /// <summary>
        /// ETB (End Of Transmission Block)
        /// 传输块结束，ASCII 0x17
        /// 表示当前中间数据帧结束，后续还有数据帧
        /// </summary>
        public const byte ETB = 0x17;

        /// <summary>
        /// ETX (End Of Text)
        /// 文本结束，ASCII 0x03
        /// 表示最后一个数据帧结束
        /// </summary>
        public const byte ETX = 0x03;

        /// <summary>
        /// 帧尾固定 5 字节：<ETX/ETB> + 2 字节校验 + <CR> + <LF>，若仪器只发 <CR> 无 <LF>，需改为 4 字节。
        /// </summary>
        public const int FrameTrailerLength = 5;

        public static bool IsFrameEnd(byte value)
        {
            return value == ASTMProtocols.ETX || value == ASTMProtocols.ETB;
        }

        public static bool IsTerminatorRecord(ReadOnlySpan<byte> record)
        {
            return record.Length >= 2
                && record[0] == (byte)'L'
                && record[1] == (byte)'|';
        }
    }
}

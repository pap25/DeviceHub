using DeviceHub.Abstractions;
using DeviceHub.Base.Common;
using DeviceHub.Base.Constant;
using DeviceHub.Base.Transports;
using DeviceHub.Lis.Dto;
using System.IO.Ports;
using System.Text;

namespace DeviceHub.Yhlo
{
    public class SerialPortDriver : ISerialDeviceDriver
    {
        private SerialPortTransport transport;
        private readonly List<byte> buffer = new();
        public async Task<Resp> Start(SerialPortConfig config)
        {
            transport = new(
                config.PortName,
                config.BaudRate,
                (Parity)config.Parity,
                config.DataBits,
                (StopBits)config.StopBits
            );
            await transport.OpenAsync();

            transport.DataReceived += Transport_DataReceived;

            return Resp.Ok();
        }

        /// <summary>
        /// 串口数据接收事件
        /// </summary>
        private async void Transport_DataReceived(byte[] data)
        {
            try
            {
                buffer.AddRange(data);

                // 提取控制字符
                while (TryExtractControlChar(out byte controlChar))
                {
                    await HandleControlCharAsync(controlChar);
                }

                // 按 ASTM 帧格式切分数据
                while (TryExtractFrame(out List<byte> frame))
                {
                    LogFrame(frame);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("串口接收数据处理异常", ex);
                buffer.Clear();
            }
        }

        /// <summary>
        /// 处理控制字符
        /// </summary>
        private async Task HandleControlCharAsync(byte controlChar)
        {
            switch (controlChar)
            {
                case ASTMProtocols.ENQ:
                    Logger.Info("收到 ENQ");
                    await transport.SendAsync(ASTMProtocols.ACK);
                    return;

                case ASTMProtocols.ACK:
                    Logger.Info("收到 ACK");
                    return;

                case ASTMProtocols.NAK:
                    Logger.Info("收到 NAK");
                    await Task.Delay(10000);
                    await transport.SendAsync(ASTMProtocols.EOT);
                    return;

                case ASTMProtocols.EOT:
                    Logger.Info("收到 EOT");
                    return;
            }
        }

        /// <summary>
        /// 按 ASTM 帧格式切分数据：
        /// 中间帧 &lt;STX&gt; FN &lt;DATA&gt; &lt;ETB&gt; &lt;CS&gt; &lt;CR&gt;&lt;LF&gt;
        /// 结束帧 &lt;STX&gt; FN &lt;DATA&gt; &lt;ETX&gt;&lt;CS&gt; &lt;CR&gt;&lt;LF&gt;
        /// </summary>
        private bool TryExtractFrame(out List<byte> frame)
        {
            frame = new List<byte>(0);

            int startIndex = IndexOfByte(ASTMProtocols.STX);
            if (startIndex < 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error($"接收数据无STX异常: {Encoding.ASCII.GetString(buffer.ToArray(), buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    buffer.Clear();
                }
                return false;
            }

            if (startIndex > 0)
                buffer.RemoveRange(0, startIndex);

            // STX + FN + ETB/ETX + CS(2) + CR + LF
            if (buffer.Count < 7)
                return false;

            for (int i = 2; i < buffer.Count; i++)
            {
                if (!ASTMProtocols.IsFrameEnd(buffer[i]))
                    continue;

                if (i + 4 >= buffer.Count)
                    return false;

                if (buffer[i + 3] != ASTMProtocols.CR || buffer[i + 4] != ASTMProtocols.LF)
                    continue;

                int frameLength = i + 5;
                frame = buffer.GetRange(0, frameLength);
                buffer.RemoveRange(0, frameLength);
                return true;
            }

            if (buffer.Count > Constants.FourMB)
            {
                Logger.Error($"接收数据无完整帧异常: {Encoding.ASCII.GetString(buffer.ToArray(), buffer.Count - Constants.OneMB, Constants.OneMB)}");
                buffer.Clear();
            }

            return false;
        }

        /// <summary>
        /// 提取控制字符
        /// </summary>
        private bool TryExtractControlChar(out byte controlChar)
        {
            controlChar = 0;

            if (buffer.Count == 0 || buffer[0] == ASTMProtocols.STX)
                return false;

            byte value = buffer[0];
            if (value is not (ASTMProtocols.ENQ or ASTMProtocols.ACK or ASTMProtocols.NAK or ASTMProtocols.EOT))
                return false;

            controlChar = value;
            buffer.RemoveAt(0);
            return true;
        }

        /// <summary>
        /// 日志记录帧
        /// </summary>
        private static void LogFrame(List<byte> frame)
        {
            int delimiterIndex = -1;
            for (int i = 2; i < frame.Count; i++)
            {
                if (ASTMProtocols.IsFrameEnd(frame[i]))
                {
                    delimiterIndex = i;
                    break;
                }
            }

            string frameType = delimiterIndex >= 0 && frame[delimiterIndex] == ASTMProtocols.ETX
                ? "结束帧"
                : "中间帧";

            string fn = frame.Count > 1 ? ((char)frame[1]).ToString() : "?";
            string payload = delimiterIndex > 2
                ? Encoding.ASCII.GetString(frame.ToArray(), 2, delimiterIndex - 2)
                : string.Empty;

            Logger.Info($"串口接收{frameType} FN={fn} DATA={payload} 原始={Encoding.ASCII.GetString(frame.ToArray())}");
        }

        /// <summary>
        /// 查找字节
        /// </summary>
        private int IndexOfByte(byte value, int startIndex = 0)
        {
            for (int i = startIndex; i < buffer.Count; i++)
            {
                if (buffer[i] == value)
                    return i;
            }

            return -1;
        }

        public void Stop()
        {
            transport.CloseAsync();
        }
    }
}
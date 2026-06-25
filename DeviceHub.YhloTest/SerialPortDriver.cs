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
        private readonly string logType = nameof(SerialPortDriver);
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

                // 按结束帧格式切分数据
                while (TryExtractFrame(out List<byte> frame))
                {
                    LogFrame(frame);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "串口接收数据处理异常", ex);
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
                    Logger.Info(logType, "收到 ENQ");
                    await transport.SendAsync(ASTMProtocols.ACK);
                    return;

                case ASTMProtocols.ACK:
                    Logger.Info(logType, "收到 ACK");
                    return;

                case ASTMProtocols.NAK:
                    Logger.Info(logType, "收到 NAK");
                    await Task.Delay(10000);
                    await transport.SendAsync(ASTMProtocols.EOT);
                    return;

                case ASTMProtocols.EOT:
                    Logger.Info(logType, "收到 EOT");
                    return;
            }
        }

        /// <summary>
        /// 按结束帧格式切分：<STX> FN &lt;DATA&gt; &lt;ETX&gt;&lt;CS&gt; &lt;CR&gt;&lt;LF&gt;
        /// 中间帧（ETB）不切分，累积至收到结束帧后一次性取出
        /// </summary>
        private bool TryExtractFrame(out List<byte> frame)
        {
            frame = new List<byte>(0);

            int startIndex = IndexOfByte(ASTMProtocols.STX);
            if (startIndex < 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error(logType, $"接收数据无STX异常: {Encoding.ASCII.GetString(buffer.ToArray(), buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    buffer.Clear();
                }
                return false;
            }

            if (startIndex > 0)
                buffer.RemoveRange(0, startIndex);

            // STX + FN + ETX + CS(2) + CR + LF
            if (buffer.Count < 7)
                return false;

            for (int i = 2; i < buffer.Count; i++)
            {
                if (buffer[i] != ASTMProtocols.ETX)
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
                Logger.Error(logType, $"接收数据无完整结束帧异常: {Encoding.ASCII.GetString(buffer.ToArray(), buffer.Count - Constants.OneMB, Constants.OneMB)}");
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
        /// 日志记录完整消息（含中间帧与结束帧）
        /// </summary>
        private void LogFrame(List<byte> frame)
        {
            Logger.Info(logType, $"串口接收完整消息 原始={Encoding.ASCII.GetString(frame.ToArray())}");

            int offset = 0;
            while (offset < frame.Count)
            {
                int stxIndex = -1;
                for (int i = offset; i < frame.Count; i++)
                {
                    if (frame[i] == ASTMProtocols.STX)
                    {
                        stxIndex = i;
                        break;
                    }
                }

                if (stxIndex < 0)
                    break;

                int delimiterIndex = -1;
                for (int i = stxIndex + 2; i < frame.Count; i++)
                {
                    if (frame[i] == ASTMProtocols.ETX || frame[i] == ASTMProtocols.ETB)
                    {
                        delimiterIndex = i;
                        break;
                    }
                }

                if (delimiterIndex < 0)
                    break;

                string frameType = frame[delimiterIndex] == ASTMProtocols.ETX ? "结束帧" : "中间帧";
                string fn = frame.Count > stxIndex + 1 ? ((char)frame[stxIndex + 1]).ToString() : "?";
                string payload = delimiterIndex > stxIndex + 2
                    ? Encoding.ASCII.GetString(frame.ToArray(), stxIndex + 2, delimiterIndex - stxIndex - 2)
                    : string.Empty;

                Logger.Info(logType, $"  └ {frameType} FN={fn} DATA={payload}");

                offset = delimiterIndex + 5;
            }
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
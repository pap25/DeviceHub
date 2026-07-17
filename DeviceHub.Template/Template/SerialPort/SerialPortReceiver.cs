using DeviceHub.Utils;
using DeviceHub.Template.Constant;
using DeviceHub.Service;
using System.Runtime.InteropServices;
using System.Text;

namespace DeviceHub.Template.Template.SerialPort
{
    public class SerialPortReceiver
    {
        private readonly string logType = nameof(SerialPortReceiver);
        private readonly SerialPortSession session;
        private readonly long instrumentId;
        private readonly IConsumeTask receiveTask;
        private readonly ReceiveMessageService receiveMessageService = ReceiveMessageService.Instance;

        private readonly List<byte> buffer = new();

        /// <summary>
        /// buffer 中下一待解析帧的起始位置（此前完整帧均已 ACK）
        /// </summary>
        private int processedOffset;

        public SerialPortReceiver(SerialPortSession session, long instrumentId, IConsumeTask receiveTask)
        {
            this.session = session;
            this.instrumentId = instrumentId;
            this.receiveTask = receiveTask;
        }

        public void OnDataReceived(byte[] data)
        {
            Logger.Debug(logType, $"串口接收消息: {Decode(data)}");

            buffer.AddRange(data);

            // 持续交替提取控制字符和完整消息，处理同一批数据中的“帧 + EOT”等组合。
            while (true)
            {
                if (TryExtractControlChar(out byte controlChar))
                {
                    session.HandleControlChar(controlChar);
                    continue;
                }

                // 按 Terminator Record（L 记录）切分完整消息
                if (TryExtractMessage(out List<byte> message))
                {
                    LogCompleteMessage(message);
                    continue;
                }

                break;
            }
        }

        public void ResetBuffer()
        {
            buffer.Clear();
            processedOffset = 0;
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
        /// 按 Terminator Record（L 记录）切分完整 ASTM 消息；多帧累积至含 L 记录后一次性取出。
        /// </summary>
        private bool TryExtractMessage(out List<byte> message)
        {
            message = new List<byte>(0);

            // buffer 三帧拼接示例：
            //   <STX>1H|\^&|||^^||||||ASCII|PR|1394-97|20180120110408<CR><ETX>A6<CR><LF>
            //   <STX>4R|1|TP^TP1I|^+|ResultUnit1|||+|F||||20171019114059|<CR><ETX>46<CR><LF>
            //   <STX>7L|1|N<CR><ETB>1E<CR><LF>
            int startIndex = IndexOfByte(ASTMProtocols.STX);
            if (startIndex < 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error(logType, $"接收数据无STX异常: {Decode(buffer, buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    ResetBuffer();
                }
                return false;
            }

            if (startIndex > 0)
            {
                buffer.RemoveRange(0, startIndex); // 丢弃 STX 前的粘包前缀
                processedOffset = Math.Max(0, processedOffset - startIndex);
            }

            int consumeLength = 0; // 命中 L 后：buffer[0] 至帧尾的总字节数

            while (processedOffset < buffer.Count)
            {
                if (buffer[processedOffset] != ASTMProtocols.STX)
                    break; // 帧对齐丢失，保留 buffer 等待更多数据或后续重同步

                if (processedOffset + 2 >= buffer.Count)
                    return false; // 半包，如只收到 <STX>1

                // processedOffset+0=<STX> +1=帧号 +2=payload 首字节(H/R/L)
                int payloadStart = processedOffset + 2;
                int frameEndIndex = -1;  // <ETX/ETB>位置

                for (int i = payloadStart; i < buffer.Count; i++)
                {
                    if (buffer[i] == ASTMProtocols.STX)
                        break; // 当前帧尚无 <ETX>/<ETB> 下一帧已开始，视为半包
                    if (ASTMProtocols.IsFrameEnd(buffer[i]))
                    {
                        frameEndIndex = i;
                        break;
                    }
                }

                if (frameEndIndex < 0)
                    return false; // 半包，等待 <ETX>/<ETB>

                int frameTrailerEnd = frameEndIndex + ASTMProtocols.FrameTrailerLength;
                if (frameTrailerEnd > buffer.Count)
                    return false; // 半包，帧尾未收齐

                session.OnFrameReceived();
                processedOffset = frameTrailerEnd;

                // 在 [payloadStart, frameEndIndex) 内按 <CR> 切记录，查找 L 记录
                int recordStart = payloadStart;
                for (int i = payloadStart; i < frameEndIndex; i++)
                {
                    if (buffer[i] != ASTMProtocols.CR)
                        continue;

                    int recordLength = i - recordStart;
                    if (ASTMProtocols.IsTerminatorRecord(CollectionsMarshal.AsSpan(buffer).Slice(recordStart, recordLength)))
                    {
                        consumeLength = frameTrailerEnd;
                        break;
                    }

                    recordStart = i + 1;
                }

                if (consumeLength > 0)
                    break;
            }

            if (consumeLength <= 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error(logType, $"接收数据无完整消息(L记录结束)异常: {Decode(buffer, buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    ResetBuffer();
                }
                return false;
            }

            message = buffer.GetRange(0, consumeLength);
            buffer.RemoveRange(0, consumeLength);
            processedOffset = 0;
            return true;
        }

        private void LogCompleteMessage(List<byte> message)
        {
            byte[] rawMessage = message.ToArray();
            Logger.Info(logType, $"串口接收完整消息{Decode(message)}");
            receiveMessageService.Save(instrumentId, rawMessage).GetAwaiter().GetResult();
            receiveTask.NotifyConsume();
        }

        private int IndexOfByte(byte value, int startIndex = 0)
        {
            for (int i = startIndex; i < buffer.Count; i++)
            {
                if (buffer[i] == value)
                    return i;
            }

            return -1;
        }

        public static string Decode(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        private static string Decode(List<byte> data)
        {
            return Decode(data.ToArray());
        }

        private static string Decode(List<byte> data, int startIndex, int count)
        {
            return count == 0 ? string.Empty : Decode(data.GetRange(startIndex, count).ToArray());
        }
    }
}

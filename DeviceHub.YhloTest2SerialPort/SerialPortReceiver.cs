using DeviceHub.Base.Common;
using DeviceHub.Base.Constant;
using DeviceHub.Service;
using System.Runtime.InteropServices;
using System.Text;

namespace DeviceHub.YhloTest2SerialPort
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

            while (TryExtractControlChar(out byte controlChar))
            {
                session.HandleControlChar(controlChar);
            }

            while (TryExtractMessage(out List<byte> message))
            {
                LogCompleteMessage(message);
            }
        }

        public void ResetBuffer()
        {
            buffer.Clear();
            processedOffset = 0;
        }

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
                buffer.RemoveRange(0, startIndex);
                processedOffset = Math.Max(0, processedOffset - startIndex);
            }

            int consumeLength = 0;

            while (processedOffset < buffer.Count)
            {
                if (buffer[processedOffset] != ASTMProtocols.STX)
                    break;

                if (processedOffset + 2 >= buffer.Count)
                    return false;

                int payloadStart = processedOffset + 2;
                int frameEndIndex = -1;

                for (int i = payloadStart; i < buffer.Count; i++)
                {
                    if (buffer[i] == ASTMProtocols.STX)
                        break;
                    if (ASTMProtocols.IsFrameEnd(buffer[i]))
                    {
                        frameEndIndex = i;
                        break;
                    }
                }

                if (frameEndIndex < 0)
                    return false;

                int frameTrailerEnd = frameEndIndex + ASTMProtocols.FrameTrailerLength;
                if (frameTrailerEnd > buffer.Count)
                    return false;

                session.OnFrameReceived();
                processedOffset = frameTrailerEnd;

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

using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Base.Common;
using DeviceHub.Base.Constant;
using DeviceHub.Base.Transports;
using DeviceHub.Model.Entities;
using DeviceHub.Service;
using DeviceHub.Yhlo.Handler;
using System.IO.Ports;
using System.Text;

namespace DeviceHub.Yhlo
{
    public class Driver : ISerialDeviceDriver
    {
        private readonly string logType = nameof(Driver);
        private IConsumeTask receiveTask;
        private readonly ReceiveMessageService receiveMessageService = ReceiveMessageService.Instance;
        private long _instrumentId;

        private SerialPortTransport transport;
        private readonly List<byte> buffer = new();

        /// <summary>
        /// 收到 NAK 后重发最后发送帧的最大次数，超过后断开连接
        /// </summary>
        private const int MaxRetransmitCount = 6;

        /// <summary>
        /// 最后一次发送的完整帧，收到 NAK 时用于重传
        /// </summary>
        private byte[]? lastSentFrame;

        /// <summary>
        /// 当前帧已重发次数
        /// </summary>
        private int retransmitCount;

        public async Task Start(long instrumentId, SerialPortConfig config)
        {
            _instrumentId = instrumentId;
            transport = new(
                    config.PortName,
                    config.BaudRate,
                    (Parity)config.Parity,
                    config.DataBits,
                    (StopBits)config.StopBits
                );

            transport.DataReceived += Transport_DataReceived;

            receiveTask = new BatchConsumeTask<ReceiveMessage>(new ReceiveHandler(instrumentId));
            receiveTask.StartConsume();

            transport.Open();
        }

        /// <summary>
        /// 串口数据接收事件
        /// </summary>
        private void Transport_DataReceived(byte[] data)
        {
            try
            {
                Logger.Debug(logType, $"串口接收消息: {Decode(data)}");

                buffer.AddRange(data);

                // 提取控制字符
                while (TryExtractControlChar(out byte controlChar))
                {
                    HandleControlCharAsync(controlChar);
                }

                // 按 Terminator Record（L 记录）切分完整消息
                while (TryExtractMessage(out List<byte> message))
                {
                    LogCompleteMessage(message);
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
        private void HandleControlCharAsync(byte controlChar)
        {
            switch (controlChar)
            {
                case ASTMProtocols.ENQ:
                    Logger.Info(logType, "收到 ENQ，回复ACK");
                    transport.Send(ASTMProtocols.ACK);
                    return;

                case ASTMProtocols.ACK:
                    Logger.Debug(logType, "收到 ACK");  // 收到ack后面可下发申请单到仪器
                    lastSentFrame = null;
                    retransmitCount = 0;
                    return;

                case ASTMProtocols.NAK:
                    Logger.Info(logType, "收到 NAK");
                    RetransmitLastFrame();
                    return;

                case ASTMProtocols.EOT:
                    Logger.Info(logType, "收到 EOT，本次传输结束，清理未完成消息");
                    return;
            }
        }

        /// <summary>
        /// 按 Terminator Record（L 记录）切分完整 ASTM 消息；中间帧（ETB）不切分，累积至收到 L 记录后一次性取出。
        /// </summary>
        private bool TryExtractMessage(out List<byte> message)
        {
            message = new List<byte>(0);

            // buffer 示例: <STX>1H|...\rP|...\rL|1|N\r<ETX>84\r\n
            int startIndex = IndexOfByte(ASTMProtocols.STX); // startIndex=0
            if (startIndex < 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error(logType, $"接收数据无STX异常: {Decode(buffer, buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    buffer.Clear();
                }
                return false;
            }

            if (startIndex > 0)
                buffer.RemoveRange(0, startIndex); // 丢弃 STX 前的粘包前缀

            int offset = 0;              // 当前解析帧起始位置，多帧时逐帧后移
            int terminatorEndIndex = -1; // L|1|N 后 <CR> 的下标
            int consumeLength = 0;       // 从 buffer 头部移除的总长度（含帧尾校验）

            while (offset < buffer.Count)
            {
                if (buffer[offset] != ASTMProtocols.STX)
                    break;

                if (offset + 2 >= buffer.Count)
                    return false; // 半包，等待 <STX>FN...

                int payloadStart = offset + 2; // 跳过 <STX>1，指向 H|... 记录区
                int frameEndIndex = -1;

                for (int i = payloadStart; i < buffer.Count; i++)
                {
                    if (ASTMProtocols.IsFrameEnd(buffer[i]))
                    {
                        frameEndIndex = i; // 单帧指向 <ETX>；中间帧指向 <ETB>
                        break;
                    }
                }

                if (frameEndIndex < 0)
                    return false; // 半包，等待 <ETX>/<ETB>

                // 在 payload 内按 <CR> 切记录，查找 L 终止记录
                // 帧1 示例 payload: H|...\rP|...\r          → 无 L，继续下一帧
                // 帧2 示例 payload: O|...\rR|...\rL|1|N\r  → 命中 L|1|N
                int recordStart = payloadStart;
                for (int i = payloadStart; i < frameEndIndex; i++)
                {
                    if (buffer[i] != ASTMProtocols.CR)
                        continue;

                    string record = Decode(buffer, recordStart, i - recordStart); // 如 "H|..." / "P|..." / "L|1|N"
                    if (ASTMProtocols.IsTerminatorRecord(record)) // "L|1|N" → true
                    {
                        terminatorEndIndex = i;              // 指向 L|1|N 后的 <CR>
                        consumeLength = frameEndIndex + 5;   // +5 = <ETX>B5\r\n 或 <ETB>A3\r\n
                        break;
                    }

                    recordStart = i + 1; // 下一条记录起始
                }

                if (terminatorEndIndex >= 0)
                    break;

                // 中间帧未含 L 记录，跳过整帧继续
                // 帧1: <STX>1H|...\rP|...\r<ETB>A3\r\n → offset 移到帧2 的 <STX>
                offset = frameEndIndex + 5;
                if (offset > buffer.Count)
                    return false; // 半包，等待下一帧
            }

            if (terminatorEndIndex < 0)
            {
                if (buffer.Count > Constants.FourMB)
                {
                    Logger.Error(logType, $"接收数据无完整消息(L记录结束)异常: {Decode(buffer, buffer.Count - Constants.OneMB, Constants.OneMB)}");
                    buffer.Clear();
                }
                return false;
            }

            if (consumeLength > buffer.Count)
                return false; // 半包，等待 <ETX><CS>\r\n 帧尾

            // message = <STX>1H|...\rP|...\r<STX>2O|...\rR|...\rL|1|N\r（至 L 记录 <CR>，不含帧尾）
            message = buffer.GetRange(0, terminatorEndIndex + 1);
            // buffer 移除 message + 末帧尾，如再移除 <ETX>B5\r\n
            buffer.RemoveRange(0, consumeLength);
            return true;
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
        /// 日志记录完整消息（至 L 记录&lt;CR&gt;，含中间帧）
        /// </summary>
        private void LogCompleteMessage(List<byte> message)
        {
            string rawMessage = Decode(message);
            Logger.Info(logType, $"串口接收完整消息 原始={rawMessage}");
            _ = receiveMessageService.Save(_instrumentId, rawMessage);
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

        private static string Decode(byte[] data)
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

        /// <summary>
        /// 发送一个完整帧，并保存为最后发送帧以便收到 NAK 时重传
        /// </summary>
        public async Task SendFrame(byte[] frame)
        {
            lastSentFrame = frame;
            retransmitCount = 0;
            transport.Send(frame);
            Logger.Debug(logType, $"串口发送帧: {Decode(frame)}");
        }

        /// <summary>
        /// 收到 NAK 后重发最后发送帧；重发超过最大次数则断开连接
        /// </summary>
        private void RetransmitLastFrame()
        {
            if (lastSentFrame == null)
            {
                Logger.Warn(logType, "收到 NAK，但没有可重发的帧");
                return;
            }

            if (retransmitCount >= MaxRetransmitCount)
            {
                lastSentFrame = null;
                retransmitCount = 0;
                Logger.Error(logType, $"收到 NAK，重发已达最大次数 {MaxRetransmitCount}，清空lastSentFrame");
                return;
            }

            retransmitCount++;
            transport.Send(lastSentFrame);
            Logger.Warn(logType, $"收到 NAK，重发最后发送帧，第 {retransmitCount}/{MaxRetransmitCount} 次");
        }

        public void Stop()
        {
            transport.Close();
        }
    }
}
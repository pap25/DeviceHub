using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Base.Common;
using DeviceHub.Base.Constant;
using DeviceHub.Base.Transports;
using DeviceHub.Model.Entities;
using DeviceHub.Service;
using DeviceHub.Yhlo.Handler;
using System.IO.Ports;
using System.Runtime.InteropServices;
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
        /// buffer 中下一待解析帧的起始位置（此前完整帧均已 ACK）
        /// </summary>
        private int processedOffset;

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
                ResetReceiveBuffer();
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
                    ResetReceiveBuffer();
                    return;
            }
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
                    ResetReceiveBuffer();
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

                transport.Send(ASTMProtocols.ACK);
                Logger.Debug(logType, "收到完整帧，回复ACK");
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
                    ResetReceiveBuffer();
                }
                return false;
            }

            message = buffer.GetRange(0, consumeLength);
            buffer.RemoveRange(0, consumeLength);
            processedOffset = 0;
            return true;
        }

        private void ResetReceiveBuffer()
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
        /// 日志记录完整 ASTM 原始报文（含多帧帧尾）
        /// </summary>
        private void LogCompleteMessage(List<byte> message)
        {
            string rawMessage = Decode(message);
            Logger.Info(logType, $"串口接收完整消息{rawMessage}");
            _ = receiveMessageService.Save(_instrumentId, rawMessage);
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
        /// 发送一个完整帧，并保存为最后发送帧以便收到 NAK 时重传 (主动向仪器下发数据（下发申请单 / 查询单）这条“发送路径”预留的，而不是接收路径)
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
            transport.DataReceived -= Transport_DataReceived;
            receiveTask.Shutdown();
            ResetReceiveBuffer();
            transport.Close();
        }
    }
}
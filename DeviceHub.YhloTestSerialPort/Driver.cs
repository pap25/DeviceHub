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
        private IConsumeTask? receiveTask;
        private readonly ReceiveMessageService receiveMessageService = ReceiveMessageService.Instance;
        private long _instrumentId;

        private SerialPortTransport? transport;
        private readonly List<byte> buffer = new();
        private readonly List<string> messageRecords = new();

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

        /// <summary>
        /// 启动串口驱动，打开串口并注册数据接收与消息消费任务
        /// </summary>
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

            await transport.Open();
        }

        /// <summary>
        /// 串口数据接收事件
        /// </summary>
        private async void Transport_DataReceived(byte[] data)
        {
            try
            {
                Logger.Debug(logType, $"串口接收消息: {decode(data)}");

                List<byte> responses = new();
                List<string> completeMessages = new();

                ProcessReceivedData(data, responses, completeMessages);

                foreach (byte response in responses)
                {
                    transport!.Send(response).GetAwaiter().GetResult();
                    Logger.Debug(logType, $"串口发送控制符: {(char)response}");
                }

                foreach (string rawMessage in completeMessages)
                {
                    Logger.Info(logType, $"串口接收完整ASTM消息: {rawMessage}");
                    await receiveMessageService.Save(_instrumentId, rawMessage);
                    receiveTask?.NotifyConsume();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "串口接收数据处理异常", ex);
                buffer.Clear();
                messageRecords.Clear();
            }
        }

        /// <summary>
        /// 解析接收到的字节流：处理 ASTM 控制符、提取帧载荷与原始记录
        /// </summary>
        private void ProcessReceivedData(byte[] data, List<byte> responses, List<string> completeMessages)
        {
            foreach (byte value in data)
            {
                switch (value)
                {
                    case ASTMProtocols.ENQ:
                        Logger.Info(logType, "收到 ENQ，回复ACK");
                        buffer.Clear();
                        messageRecords.Clear();
                        responses.Add(ASTMProtocols.ACK);
                        break;

                    case ASTMProtocols.EOT:
                        Logger.Info(logType, "收到 EOT，本次传输结束，清理未完成消息");
                        buffer.Clear();
                        messageRecords.Clear();
                        break;

                    case ASTMProtocols.ACK:
                        Logger.Debug(logType, "收到 ACK");
                        lastSentFrame = null;
                        retransmitCount = 0;
                        break;

                    case ASTMProtocols.NAK:
                        Logger.Debug(logType, "收到 NAK");
                        RetransmitLastFrame();
                        break;

                    default:
                        buffer.Add(value);
                        break;
                }
            }

            while (TryExtractFramePayload(out string payload))
            {
                AppendPayloadRecords(payload, completeMessages);
                responses.Add(ASTMProtocols.ACK);
            }

            ExtractRawRecords(completeMessages);
            GuardBufferSize();
        }

        /// <summary>
        /// 从缓冲区提取一帧 STX...ETX/ETB 载荷；帧不完整时返回 false
        /// </summary>
        private bool TryExtractFramePayload(out string payload)
        {
            payload = string.Empty;

            int startIndex = IndexOfByte(ASTMProtocols.STX);
            if (startIndex < 0)
            {
                return false;
            }

            if (startIndex > 0)
            {
                Logger.Warn(logType, $"ASTM帧前存在无效数据，已丢弃长度: {startIndex}");
                buffer.RemoveRange(0, startIndex);
            }

            int endIndex = IndexOfFrameEnd(1);
            if (endIndex < 0)
            {
                return false;
            }

            int consumed = GetFrameConsumedLength(endIndex);
            if (consumed < 0)
            {
                return false;
            }

            int payloadStart = 1;
            int payloadLength = endIndex - payloadStart;
            byte[] payloadBytes = buffer.GetRange(payloadStart, payloadLength).ToArray();

            if (payloadBytes.Length > 0 && payloadBytes[0] >= '0' && payloadBytes[0] <= '7')
            {
                payload = decode(payloadBytes[1..]);
            }
            else
            {
                payload = decode(payloadBytes);
            }

            buffer.RemoveRange(0, consumed);
            return true;
        }

        /// <summary>
        /// 在无 STX 帧时，按 CR/LF 从缓冲区逐条提取原始 ASTM 记录
        /// </summary>
        private void ExtractRawRecords(List<string> completeMessages)
        {
            if (IndexOfByte(ASTMProtocols.STX) >= 0)
            {
                return;
            }

            while (true)
            {
                int recordEndIndex = IndexOfByte(ASTMProtocols.CR);
                if (recordEndIndex < 0)
                {
                    return;
                }

                byte[] recordBytes = buffer.GetRange(0, recordEndIndex).ToArray();
                int consumed = recordEndIndex + 1;
                if (consumed < buffer.Count && buffer[consumed] == ASTMProtocols.LF)
                {
                    consumed++;
                }

                buffer.RemoveRange(0, consumed);
                AppendRecord(decode(recordBytes), completeMessages);
            }
        }

        /// <summary>
        /// 将帧载荷按 CR 拆分为多条记录并追加
        /// </summary>
        private void AppendPayloadRecords(string payload, List<string> completeMessages)
        {
            string[] records = payload.Split((char)ASTMProtocols.CR, StringSplitOptions.RemoveEmptyEntries);
            foreach (string record in records)
            {
                AppendRecord(record.TrimEnd((char)ASTMProtocols.LF), completeMessages);
            }
        }

        /// <summary>
        /// 追加单条记录；遇到 L 终止记录时组装完整消息
        /// </summary>
        private void AppendRecord(string record, List<string> completeMessages)
        {
            if (string.IsNullOrWhiteSpace(record))
            {
                return;
            }

            messageRecords.Add(record);

            if (!ASTMProtocols.IsTerminatorRecord(record))
            {
                return;
            }

            completeMessages.Add(string.Join("\r", messageRecords) + "\r");
            messageRecords.Clear();
        }

        /// <summary>
        /// 计算从帧起始到帧尾（含校验与 CR/LF）应消费的字节数
        /// </summary>
        private int GetFrameConsumedLength(int endIndex)
        {
            int index = endIndex + 1;
            if (index >= buffer.Count)
            {
                return -1;
            }

            if (buffer[index] == ASTMProtocols.CR)
            {
                index++;
                if (index < buffer.Count && buffer[index] == ASTMProtocols.LF)
                {
                    index++;
                }
                return index;
            }

            if (index + 1 >= buffer.Count)
            {
                return -1;
            }

            if (IsAsciiHex(buffer[index]) && IsAsciiHex(buffer[index + 1]))
            {
                index += 2;
                if (index >= buffer.Count)
                {
                    return -1;
                }

                if (buffer[index] == ASTMProtocols.CR)
                {
                    index++;
                    if (index < buffer.Count && buffer[index] == ASTMProtocols.LF)
                    {
                        index++;
                    }
                }
                else if (buffer[index] == ASTMProtocols.LF)
                {
                    index++;
                }
            }

            return index;
        }

        /// <summary>
        /// 缓冲区超过上限时清空，防止异常数据导致内存膨胀
        /// </summary>
        private void GuardBufferSize()
        {
            if (buffer.Count <= Constants.FourMB)
            {
                return;
            }

            Logger.Error(logType, $"ASTM接收缓冲区超过限制，清空缓冲区。末尾数据: {decode(buffer.GetRange(buffer.Count - Constants.OneMB, Constants.OneMB).ToArray())}");
            buffer.Clear();
            messageRecords.Clear();
        }

        /// <summary>
        /// 从指定位置起查找 ETX 或 ETB 帧结束符
        /// </summary>
        private int IndexOfFrameEnd(int startIndex)
        {
            for (int i = startIndex; i < buffer.Count; i++)
            {
                if (ASTMProtocols.IsFrameEnd(buffer[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 在缓冲区中查找指定字节的位置
        /// </summary>
        private int IndexOfByte(byte value, int startIndex = 0)
        {
            for (int i = startIndex; i < buffer.Count; i++)
            {
                if (buffer[i] == value)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 判断字节是否为 ASCII 十六进制字符（0-9、A-F、a-f）
        /// </summary>
        private static bool IsAsciiHex(byte value)
        {
            return value is >= (byte)'0' and <= (byte)'9' or
                   >= (byte)'A' and <= (byte)'F' or
                   >= (byte)'a' and <= (byte)'f';
        }

        /// <summary>
        /// 停止驱动：关闭消息消费任务并释放串口资源
        /// </summary>
        public void Stop()
        {
            receiveTask?.Shutdown();

            if (transport != null)
            {
                transport.DataReceived -= Transport_DataReceived;
                transport.Close().GetAwaiter().GetResult();
                transport.Dispose();
            }
        }

        /// <summary>
        /// 将字节数组解码为 UTF-8 字符串
        /// </summary>
        private string decode(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// 发送一个完整帧，并保存为最后发送帧以便收到 NAK 时重传
        /// </summary>
        public async Task SendFrame(byte[] frame)
        {
            lastSentFrame = frame;
            retransmitCount = 0;
            await transport!.Send(frame);
            Logger.Debug(logType, $"串口发送帧: {decode(frame)}");
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
                Logger.Error(logType, $"收到 NAK，重发已达最大次数 {MaxRetransmitCount}，断开连接");
                lastSentFrame = null;
                retransmitCount = 0;
                transport!.Close().GetAwaiter().GetResult();
                return;
            }

            retransmitCount++;
            transport!.Send(lastSentFrame).GetAwaiter().GetResult();
            Logger.Warn(logType, $"收到 NAK，重发最后发送帧，第 {retransmitCount}/{MaxRetransmitCount} 次");
        }
    }
}
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
        private readonly ReceiveMessageService receiveMessageService = ReceiveMessageService.Instance;
        private readonly List<byte> receiveBuffer = new();
        private readonly List<string> currentRecords = new();
        private readonly StringBuilder recordBuilder = new();

        private IConsumeTask receiveTask = null!;
        private SerialPortTransport transport = null!;
        private long instrumentId;
        private int expectedFrameNumber = 1;

        public async Task Start(long instrumentId, SerialPortConfig config)
        {
            this.instrumentId = instrumentId;
            transport = new(
                config.PortName,
                config.BaudRate,
                (Parity)config.Parity,
                config.DataBits,
                (StopBits)config.StopBits);

            transport.DataReceived += Transport_DataReceived;

            receiveTask = new BatchConsumeTask<ReceiveMessage>(new ReceiveHandler(instrumentId));
            receiveTask.StartConsume();

            await transport.Open();
        }

        /// <summary>
        /// 串口数据接收事件（单仪器单服务，串口回调同步处理，无需加锁）
        /// </summary>
        private void Transport_DataReceived(byte[] data)
        {
            try
            {
                Logger.Debug(logType, $"串口接收消息: {Encoding.UTF8.GetString(data)}");

                receiveBuffer.AddRange(data);

                List<byte> responses = new();
                List<string> completeMessages = new();
                _ = ProcessReceiveBuffer(responses, completeMessages);

                foreach (byte response in responses)
                {
                    transport.Send(response);
                    Logger.Debug(logType, $"串口发送控制符: {ToControlName(response)}");
                }

                foreach (string rawMessage in completeMessages)
                {
                    Logger.Info(logType, $"串口接收完整消息: {rawMessage}");
                    _ = receiveMessageService.Save(instrumentId, rawMessage);
                    receiveTask.NotifyConsume();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "串口接收数据处理异常", ex);
            }
        }

        private async Task ProcessReceiveBuffer(List<byte> responses, List<string> completeMessages)
        {
            while (receiveBuffer.Count > 0)
            {
                byte first = receiveBuffer[0];

                if (first != ASTMProtocols.STX)
                {
                    // 处理控制字符情况
                    await ProcessControlOrDiscard(first);
                    receiveBuffer.RemoveAt(0);
                    continue;
                }

                int frameEndIndex = IndexOfFrameEnd(2);
                // 没有ETX/ETB
                if (frameEndIndex < 0)
                {
                    ClearBufferIfTooLarge("接收数据没ETX/ETB异常");
                    return;
                }

                // 有ETX/ETB
                if (!TryGetFrameLength(frameEndIndex, out int frameLength)) // 继续等待
                {
                    ClearBufferIfTooLarge("接收数据帧未接收完整异常");
                    return;
                }

                if (!ValidateFrameNumber(receiveBuffer[1]))
                {
                    Logger.Warn(logType, $"ASTM帧号错误, expected:{expectedFrameNumber}, actual:{(char)receiveBuffer[1]}");
                    responses.Add(ASTMProtocols.NAK);
                    receiveBuffer.RemoveRange(0, frameLength);
                    continue;
                }

                string frameData = Encoding.ASCII.GetString(CollectionsMarshal.AsSpan(receiveBuffer)[2..frameEndIndex]);
                AppendFrameData(frameData, completeMessages);

                expectedFrameNumber = NextFrameNumber(expectedFrameNumber);
                responses.Add(ASTMProtocols.ACK);
                receiveBuffer.RemoveRange(0, frameLength);
            }
        }

        private async Task ProcessControlOrDiscard(byte value)
        {
            switch (value)
            {
                case ASTMProtocols.ENQ:
                    Logger.Debug(logType, "收到ENQ，回复ACK");
                    ResetReceiveState();
                    await transport.Send(ASTMProtocols.ACK);
                    break;
                case ASTMProtocols.EOT:
                    Logger.Debug(logType, "收到EOT, 本次ASTM传输结束");
                    ResetReceiveState();
                    break;
                case ASTMProtocols.ACK:
                    Logger.Debug(logType, $"收到ACK"); // 后续补充下发申请单
                    break;
                case ASTMProtocols.NAK:
                    Logger.Debug(logType, $"收到NAK");
                    ScheduleEnqAfterDelay();
                    break;
                case ASTMProtocols.LF:
                case ASTMProtocols.CR:
                    break;
                default:
                    Logger.Warn(logType, $"丢弃非ASTM帧数据: 0x{value:X2}");
                    break;
            }
        }

        private void ScheduleEnqAfterDelay()
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(10000);
                await transport.Send(ASTMProtocols.ENQ);
                Logger.Debug(logType, "收到NAK后重新发送ENQ");
            });
        }

        private void AppendFrameData(string frameData, List<string> completeMessages)
        {
            foreach (char item in frameData)
            {
                if (item == (char)ASTMProtocols.CR)
                {
                    string record = recordBuilder.ToString();
                    recordBuilder.Clear();

                    if (record.Length == 0)
                    {
                        continue;
                    }

                    currentRecords.Add(record);

                    if (IsTerminatorRecord(record))
                    {
                        completeMessages.Add(string.Join("\r", currentRecords) + "\r");
                        currentRecords.Clear();
                    }

                    continue;
                }

                if (item == (char)ASTMProtocols.LF && recordBuilder.Length == 0)
                {
                    continue;
                }

                recordBuilder.Append(item);
            }
        }

        private bool TryGetFrameLength(int frameEndIndex, out int frameLength)
        {
            frameLength = 0;

            // STX + FN + ETB/ETX + CS(2) + CR + LF 没有后面 CS + CR 则继续等待
            if (receiveBuffer.Count <= frameEndIndex + 3)
            {
                return false;
            }

            if (receiveBuffer[frameEndIndex + 3] != ASTMProtocols.CR)
            {
                Logger.Warn(logType, $"ASTM帧校验尾部格式异常: {Encoding.UTF8.GetString(CollectionsMarshal.AsSpan(receiveBuffer)[..Math.Min(receiveBuffer.Count, frameEndIndex + 4)])}");
                frameLength = frameEndIndex + 4;
                return true;
            }

            if (receiveBuffer.Count <= frameEndIndex + 4)
            {
                return false;
            }

            frameLength = receiveBuffer[frameEndIndex + 4] == ASTMProtocols.LF
                ? frameEndIndex + 5
                : frameEndIndex + 4;

            return true;
        }

        private bool ValidateFrameNumber(byte frameNumber)
        {
            return frameNumber >= (byte)'0' &&
                   frameNumber <= (byte)'7' &&
                   frameNumber == (byte)('0' + expectedFrameNumber);
        }

        private static int NextFrameNumber(int frameNumber) => frameNumber == 7 ? 0 : frameNumber + 1;

        private static bool IsTerminatorRecord(string record) => record.StartsWith("L|", StringComparison.Ordinal);

        private int IndexOfFrameEnd(int startIndex)
        {
            for (int i = startIndex; i < receiveBuffer.Count; i++)
            {
                if (ASTMProtocols.IsFrameEnd(receiveBuffer[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        private void ClearBufferIfTooLarge(string message)
        {
            if (receiveBuffer.Count <= Constants.FourMB)
            {
                return;
            }

            int logLength = Math.Min(Constants.OneMB, receiveBuffer.Count);
            Logger.Error(logType, $"{message}: {Encoding.ASCII.GetString(CollectionsMarshal.AsSpan(receiveBuffer)[^logLength..])}");
            receiveBuffer.Clear();
            ResetReceiveState();
        }

        private void ResetReceiveState()
        {
            expectedFrameNumber = 1;
            currentRecords.Clear();
            recordBuilder.Clear();
        }

        private static string ToControlName(byte value) => value switch
        {
            ASTMProtocols.ENQ => "ENQ",
            ASTMProtocols.ACK => "ACK",
            ASTMProtocols.EOT => "EOT",
            ASTMProtocols.NAK => "NAK",
            ASTMProtocols.STX => "STX",
            ASTMProtocols.LF => "LF",
            ASTMProtocols.CR => "CR",
            ASTMProtocols.ETB => "ETB",
            ASTMProtocols.ETX => "ETX",
            _ => $"0x{value:X2}"
        };

        public void Stop()
        {
            transport.Close();
        }
    }
}

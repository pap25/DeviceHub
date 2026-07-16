using DeviceHub.Utils;
using System.IO.Ports;
using System.Text;

namespace DeviceHub.Template.Transports
{
    public class SerialPortTransport : IDisposable
    {
        private readonly string logType = nameof(SerialPortTransport);
        private readonly SerialPort _serialPort;
        private CancellationTokenSource? _receiveCts;
        private Task? _receiveTask;

        public event Action<byte[]>? DataReceived;

        public bool IsOpen => _serialPort.IsOpen;

        public SerialPortTransport(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

            Logger.Info(logType, $"初始化串口 portName:{portName}, baudRate:{baudRate}, parity:{parity}, " +
                $"dataBits:{dataBits}, stopBits:{stopBits}");
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
                StartReceiveLoop();
                Logger.Info(logType, $"串口已打开: {_serialPort.PortName}");
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            RequestReceiveLoopStop();

            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                Logger.Info(logType, $"串口已关闭: {_serialPort.PortName}");
            }

            WaitReceiveLoopStopped();
        }

        /// <summary>
        /// 发送字节
        /// </summary>
        public void Send(byte[] data)
        {
            if (!_serialPort.IsOpen)
            {
                throw new InvalidOperationException("串口未打开");
            }

            _serialPort.Write(data, 0, data.Length);
        }

        public void Send(byte data)
        {
            Send([data]);
        }

        /// <summary>
        /// 发送字符串
        /// </summary>
        public void Send(string message, Encoding? encoding = null)
        {
            encoding ??= Encoding.ASCII;

            Send(encoding.GetBytes(message));
        }

        private void StartReceiveLoop()
        {
            if (_receiveTask is { IsCompleted: false })
            {
                return;
            }

            _receiveCts = new CancellationTokenSource();
            CancellationToken token = _receiveCts.Token;
            _receiveTask = Task.Factory.StartNew(
                () => ReceiveLoop(token),
                token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        private void ReceiveLoop(CancellationToken token)
        {
            byte[] buffer = new byte[4096];

            while (!token.IsCancellationRequested)
            {
                try
                {
                    int count = _serialPort.Read(buffer, 0, buffer.Length);

                    if (count <= 0)
                    {
                        continue;
                    }

                    byte[] data = new byte[count];
                    Buffer.BlockCopy(buffer, 0, data, 0, count);
                    DataReceived?.Invoke(data);
                }
                catch (TimeoutException)
                {
                    // 保留兼容：如果外部配置了 ReadTimeout，超时后继续等待。
                }
                catch (InvalidOperationException) when (token.IsCancellationRequested)
                {
                    break;
                }
                catch (ObjectDisposedException) when (token.IsCancellationRequested)
                {
                    break;
                }
                catch (IOException) when (token.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error(logType, "串口接收循环异常", ex);
                    break;
                }
            }
        }

        private void RequestReceiveLoopStop()
        {
            _receiveCts?.Cancel();
        }

        private void WaitReceiveLoopStopped()
        {
            try
            {
                if (_receiveTask != null && Task.CurrentId != _receiveTask.Id)
                {
                    _receiveTask.Wait(TimeSpan.FromSeconds(2));
                }
            }
            catch (AggregateException ex)
            {
                Logger.Warn(logType, $"等待串口接收循环结束异常: {ex.InnerException?.Message ?? ex.Message}");
            }
            finally
            {
                _receiveCts?.Dispose();
                _receiveCts = null;
                _receiveTask = null;
            }
        }

        public void Dispose()
        {
            if (_serialPort.IsOpen)
            {
                Close();
            }
            else
            {
                RequestReceiveLoopStop();
                WaitReceiveLoopStopped();
            }

            _serialPort.Dispose();

            Logger.Info(logType, $"串口 Dispose: {_serialPort.PortName}");
        }
    }
}

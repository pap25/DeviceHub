using DeviceHub.Utils;
using System.Text;
using System.Net.Sockets;

namespace DeviceHub.Base.Transports
{
    public class TcpClientTransport : IDisposable
    {
        private readonly string logType = nameof(TcpClientTransport);
        private readonly string _host;
        private readonly int _port;

        private TcpClient? _client;
        private NetworkStream? _stream;

        public event Action<byte[]>? DataReceived;

        public bool IsConnected => _client?.Connected ?? false;

        public TcpClientTransport(string host, int port)
        {
            _host = host;
            _port = port;
            Logger.Info(logType, $"初始化TCP客户端 host:{host}, port:{port}");
        }

        public async Task ConnectAsync()
        {
            _client = new TcpClient();

            await _client.ConnectAsync(_host, _port);

            Logger.Info(logType, $"TCP客户端已连接 host:{_host}, port:{_port}");

            _stream = _client.GetStream();

            await Task.Run(ReceiveLoop);
        }

        public Task Disconnect()
        {
            _stream?.Close();
            _client?.Close();

            Logger.Info(logType, $"TCP客户端断开连接 host:{_host}, port:{_port}");

            return Task.CompletedTask;
        }

        public async Task SendAsync(byte[] data)
        {
            if (_stream == null)
                throw new InvalidOperationException("TCP客户端未连接");

            await _stream.WriteAsync(data);
        }

        public Task SendAsync(string message, Encoding? encoding = null)
        {
            encoding ??= Encoding.ASCII;

            return SendAsync(encoding.GetBytes(message));
        }

        private async Task ReceiveLoop()
        {
            var buffer = new byte[4096];

            try
            {
                while (true)
                {
                    int len = await _stream!.ReadAsync(buffer);

                    if (len == 0)
                        break;

                    var data = buffer[..len];

                    DataReceived?.Invoke(data);
                }
            }
            catch (ObjectDisposedException ex)
            {
                Logger.Error(logType, "TCP客户端 ObjectDisposedException 异常", ex);
            }
            catch (IOException ex)
            {
                Logger.Error(logType, "TCP客户端 IOException 异常", ex);
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "TCP客户端 Exception 异常", ex);
            }
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _client?.Dispose();

            Logger.Info(logType, $"TCP客户端 Dispose host:{_host}, port:{_port}");
        }
    }
}
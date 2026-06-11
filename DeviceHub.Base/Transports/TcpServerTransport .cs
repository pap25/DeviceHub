using DeviceHub.Base.Common;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DeviceHub.Base.Transports
{
    public class TcpServerTransport : IDisposable
    {
        private readonly string _host;
        private readonly int _port;

        private TcpListener? _listener;
        private TcpClient? _client;
        private NetworkStream? _stream;

        public event Action<byte[]>? DataReceived;

        public bool IsConnected => _client?.Connected ?? false;

        public TcpServerTransport(string host, int port)
        {
            _host = host;
            _port = port;
            Logger.Info($"初始化TCP服务端 host:{host}, port:{port}");
        }

        public async Task StartAsync()
        {
            var ip = string.IsNullOrWhiteSpace(_host) || _host == "0.0.0.0" || _host == "*"
                ? IPAddress.Any
                : IPAddress.Parse(_host);

            _listener = new TcpListener(ip, _port);

            _listener.Start();

            _client = await _listener.AcceptTcpClientAsync();

            Logger.Info($"客户端已连接: {_client.Client.RemoteEndPoint}, host:{_host}, port:{_port}");

            _stream = _client.GetStream();

            _ = Task.Run(ReceiveLoop);
        }

        public Task StopAsync()
        {
            _stream?.Close();
            _client?.Close();
            _listener?.Stop();

            Logger.Info($"TCP服务端已停止 host:{_host}, port:{_port}");

            return Task.CompletedTask;
        }

        public async Task SendAsync(byte[] data)
        {
            if (_stream == null)
                throw new InvalidOperationException("TCP服务未就绪或客户端尚未连接");

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
                    {
                        Logger.Info($"客户端已断开连接: {_client?.Client.RemoteEndPoint}, host:{_host}, port:{_port}");
                        break;
                    }

                    var data = buffer[..len];

                    DataReceived?.Invoke(data); // 事件处理要考虑半包、粘包
                }
            }
            catch (ObjectDisposedException ex)
            {
                Logger.Error("TCP服务端 ObjectDisposedException", ex);
            }
            catch (IOException ex)
            {
                Logger.Error("TCP服务端 IOException", ex);
            }
            catch (Exception ex)
            {
                Logger.Error("TCP服务端 Exception", ex);
            }
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _client?.Dispose();
            _listener?.Stop();

            Logger.Info($"TCP服务端 Dispose host:{_host}, port:{_port}");
        }
    }
}
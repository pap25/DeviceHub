using DeviceHub.Base.Common;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DeviceHub.Base.Transports
{
    public class TcpServerTransport : IDisposable
    {
        private readonly string logType = nameof(TcpServerTransport);
        private readonly string _host;
        private readonly int _port;

        private TcpListener? _listener;
        private TcpClient? _client;
        private NetworkStream? _stream;

        private readonly SemaphoreSlim _sendLock = new(1, 1);
        private readonly CancellationTokenSource _cts = new();

        public event Action<byte[]>? DataReceived;
        private string _clientRemoteEndPoint = string.Empty;

        public string GetClientRemoteEndPoint()
        {
            return _clientRemoteEndPoint;
        }

        //public bool IsConnected
        //{
        //    get
        //    {
        //        try
        //        {
        //            return _client != null && _client.Client != null
        //                && !(_client.Client.Poll(1, SelectMode.SelectRead) && _client.Client.Available == 0);
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        public TcpServerTransport(string host, int port)
        {
            _host = host;
            _port = port;

            Logger.Info(logType, $"初始化TCP服务端 host:{host}, port:{port}");
        }

        public async Task StartListeningAsync()
        {
            var ip = string.IsNullOrWhiteSpace(_host)
                || _host == "0.0.0.0"
                || _host == "*"
                    ? IPAddress.Any
                    : IPAddress.Parse(_host);

            _listener = new TcpListener(ip, _port);

            _listener.Start();

            Logger.Info(logType, $"TCP服务端开始监听 host:{_host}, port:{_port}");

            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    TcpClient client;

                    try
                    {
                        client = await _listener.AcceptTcpClientAsync(_cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }

                    await _sendLock.WaitAsync(_cts.Token);

                    try
                    {
                        if (_client != null)
                        {
                            var oldClient = _client;
                            var oldStream = _stream;
                            var newEndPoint = client.Client.RemoteEndPoint?.ToString() ?? "unknown";

                            Logger.Warn(logType, $"新连接替换旧连接: 旧={oldClient.Client.RemoteEndPoint}, 新={newEndPoint}, host:{_host}, port:{_port}");

                            try
                            {
                                oldStream?.Close();
                                oldClient.Close();
                            }
                            catch
                            {
                            }
                        }

                        _client = client;
                        _stream = client.GetStream();
                        _clientRemoteEndPoint = client.Client.RemoteEndPoint?.ToString() ?? string.Empty;

                        Logger.Info(logType, $"客户端已连接: {_clientRemoteEndPoint}, host:{_host}, port:{_port}");
                    }
                    finally
                    {
                        _sendLock.Release();
                    }

                    _ = Task.Run(() => ReceiveLoopAsync(client, _stream, _cts.Token));
                }
            }
            catch (ObjectDisposedException)
            {
                // Stop时正常退出
            }
            catch (SocketException)
            {
                // Stop时正常退出
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "TCP服务端监听异常", ex);
            }

            Logger.Info(logType, $"TCP服务端监听结束 host:{_host}, port:{_port}");
        }

        public void Stop()
        {
            string remoteEndPoin = string.Empty;
            try
            {
                if (_client != null)
                {
                    remoteEndPoin = _client.Client.RemoteEndPoint?.ToString() ?? "unknown";
                }
                _cts.Cancel();

                _stream?.Close();
                _client?.Close();
                _listener?.Stop();
            }
            finally
            {
                _stream = null;
                _client = null;
                _listener = null;
            }

            Logger.Info(logType, $"TCP服务端已停止, 客户端端点:{remoteEndPoin} host:{_host}, port:{_port}");
        }

        public async Task SendAsync(byte[] data)
        {
            if (_stream == null)
                throw new InvalidOperationException("TCP服务未就绪或客户端尚未连接");

            await _sendLock.WaitAsync();

            try
            {
                await _stream.WriteAsync(data, _cts.Token);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        public Task SendAsync(string message, Encoding? encoding = null)
        {
            encoding ??= Encoding.ASCII;

            return SendAsync(encoding.GetBytes(message));
        }

        private async Task ReceiveLoopAsync(TcpClient client, NetworkStream stream, CancellationToken cancellationToken)
        {
            var buffer = new byte[4096];

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    int len = await stream.ReadAsync(
                        buffer,
                        cancellationToken);

                    if (len == 0)
                    {
                        Logger.Info(logType, $"客户端已断开连接: {client.Client.RemoteEndPoint}, host:{_host}, port:{_port}");
                        break;
                    }

                    var data = buffer[..len];

                    // 上层负责处理半包、粘包
                    DataReceived?.Invoke(data);
                }
            }
            catch (OperationCanceledException)
            {
                // Stop时正常退出
            }
            catch (ObjectDisposedException ex)
            {
                Logger.Error(logType, $"TCP服务端 ObjectDisposedException 客户端端点:{client.Client.RemoteEndPoint}", ex);
            }
            catch (IOException ex)
            {
                Logger.Error(logType, "TCP服务端 IOException 客户端端点:{client.Client.RemoteEndPoint}", ex);
            }
            catch (Exception ex)
            {
                Logger.Error(logType, "TCP服务端 Exception 客户端端点:{client.Client.RemoteEndPoint}", ex);
            }
            finally
            {
                try
                {
                    stream.Dispose();
                    client.Dispose();
                }
                catch
                {
                }

                // 防止误清理后续连接
                if (ReferenceEquals(client, _client))
                {
                    _stream = null;
                    _client = null;
                }

                Logger.Info(logType, $"客户端资源已释放, 客户端端点:{client.Client.RemoteEndPoint}, host:{_host}, port:{_port}");
            }
        }

        public void Dispose()
        {
            Stop();

            _cts.Dispose();
            _sendLock.Dispose();

            Logger.Info(logType, $"TCP服务端 Dispose host:{_host}, port:{_port}");
        }
    }
}
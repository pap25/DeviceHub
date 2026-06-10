using DeviceHub.Base.Common;
using System.Text;
using System;
using System.IO.Ports;
using System.Net.Sockets;

namespace DeviceHub.Base.Transports
{
    namespace DeviceHub.Base.Transports
    {
        public class TcpTransport1 : IDisposable
        {
            private readonly string _host;
            private readonly int _port;

            private TcpClient? _client;
            private NetworkStream? _stream;

            public event Action<byte[]>? DataReceived;

            public bool IsConnected => _client?.Connected ?? false;

            public TcpTransport1(string host, int port)
            {
                _host = host;
                _port = port;
                Logger.Info($"初始化网口TCP: host={host}, port={port}");
            }

            public async Task ConnectAsync()
            {
                _client = new TcpClient();

                await _client.ConnectAsync(_host, _port);

                Logger.Info($"网口TCP已连接: host={_host}, port={_port}");

                _stream = _client.GetStream();

                _ = Task.Run(ReceiveLoop);
            }

            public Task DisconnectAsync()
            {
                _stream?.Close();
                _client?.Close();

                Logger.Info($"网口TCP断开连接: host={_host}, port={_port}");

                return Task.CompletedTask;
            }

            public async Task SendAsync(byte[] data)
            {
                if (_stream == null)
                    throw new InvalidOperationException("网口TCP未连接");

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
                    Logger.Error("网口TCP ObjectDisposedException 异常", ex);
                }
                catch (IOException ex)
                {
                    Logger.Error("网口TCP IOException 异常", ex);
                }
                catch (Exception ex)
                {
                    Logger.Error("网口TCP Exception 异常", ex);
                }
            }

            public void Dispose()
            {
                _stream?.Dispose();
                _client?.Dispose();

                Logger.Info($"网口TCP Dispose: host={_host}, port={_port}");
            }
        }
    }
}

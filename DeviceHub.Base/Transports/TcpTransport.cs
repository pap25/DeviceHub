using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace DeviceHub.Base.Transports
{
    using System;
    using System.Net.Sockets;

    namespace DeviceHub.Base.Transports
    {
        public class TcpTransport : IDisposable
        {
            private readonly string _host;
            private readonly int _port;

            private TcpClient? _client;
            private NetworkStream? _stream;

            public event Action<byte[]>? DataReceived;

            public bool IsConnected =>_client?.Connected ?? false;

            public TcpTransport(string host, int port)
            {
                _host = host;
                _port = port;
            }

            public async Task ConnectAsync()
            {
                _client = new TcpClient();

                await _client.ConnectAsync(_host, _port);

                _stream = _client.GetStream();

                _ = Task.Run(ReceiveLoop);
            }

            public Task DisconnectAsync()
            {
                _stream?.Close();
                _client?.Close();

                return Task.CompletedTask;
            }

            public async Task SendAsync(byte[] data)
            {
                if (_stream == null)
                    throw new InvalidOperationException("未连接");

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
                catch (ObjectDisposedException)
                {
                }
                catch (IOException)
                {
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            public void Dispose()
            {
                _stream?.Dispose();
                _client?.Dispose();
            }
        }
    }
}

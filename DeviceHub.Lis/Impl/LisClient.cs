using DeviceHub.Lis.Dto;
using System.Text.Json;

namespace DeviceHub.Lis.Impl
{
    public class LisClient : ILisClient
    {
        private static readonly LisClient _instance = new();

        public static LisClient Instance => _instance;

        private LisClient()
        {
        }

        public async Task<DriverConfig> queryDriverConfig(int id)
        {
            //await Task.Delay(3000); // 模拟网络IO
            //if (id == 1)
            //{
            //    throw new Exception("调用LIS异常");
            //}

            const string serialPortConfigFile = "LIS/serialPortConfig.json";
            const string tcpConfigFile = "LIS/tcpConfig.json";

            SerialPortConfig? serialPortConfig = null;
            TcpConfig? tcpConfig = null;

            if (File.Exists(serialPortConfigFile))
            {
                try
                {
                    string json = await File.ReadAllTextAsync(serialPortConfigFile);
                    serialPortConfig = JsonSerializer.Deserialize<SerialPortConfig>(json);
                }
                catch
                {
                    serialPortConfig = null;
                }
            }

            if (File.Exists(tcpConfigFile))
            {
                try
                {
                    string json = await File.ReadAllTextAsync(tcpConfigFile);
                    tcpConfig = JsonSerializer.Deserialize<TcpConfig>(json);
                }
                catch
                {
                    tcpConfig = null;
                }
            }

            return new DriverConfig
            {
                TcpConfig = tcpConfig,
                SerialPortConfig = serialPortConfig
            };
        }
    }
}

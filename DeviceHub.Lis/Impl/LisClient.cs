using DeviceHub.Lis.Dto;

namespace DeviceHub.Lis.Impl
{
    public class LisClient : ILisClient
    {
        public async Task<DriverConfig> queryDriverConfig(int id)
        {
            //await Task.Delay(3000); // 模拟网络IO
            //if (id == 1)
            //{
            //    throw new Exception("调用LIS异常");
            //}
            var serialPortConfig = new SerialPortConfig
            {
                PortName = "COM4",
                BaudRate = 9600,
                Parity = 0,
                DataBits = 8,
                StopBits = 1
            };

            var tcpConfig = new TcpConfig
            {
                Host = "172.23.0.1",
                Port = 5000,
            };

            DriverConfig config = new DriverConfig
            {
                TcpConfig = tcpConfig,
                SerialPortConfig = serialPortConfig
            };

            return config;
        }
    }
}

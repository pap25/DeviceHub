using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis.dto;
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

        public Task<GetInstrument> GetInstrument(long instrumentId)
        {
            throw new NotImplementedException();
        }

        public async Task<DriverConfig> GetDriverConfig(long instrumentId)
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

        public Task<Resp<UploadSpecimenTestResultOutput>> UploadSpecimenTestResult(UploadSpecimenTestResultInput uploadSpecimenTestResultInput)
        {
            throw new NotImplementedException();
        }

        public Task<GetSampleApplyItemOutput> GetSampleApplyItem(string sampleNo, string barcode)
        {
            throw new NotImplementedException();
        }

        public Task<List<GetSampleApplyListOutput>> GetSampleApplyList(long instrumentId, long lastId, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}

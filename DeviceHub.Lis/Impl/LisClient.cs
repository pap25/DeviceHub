using DeviceHub.Abstractions.Dto;
using DeviceHub.Utils;
using DeviceHub.Lis.Dto;
using System.Text.Json;
using static DeviceHub.Lis.Dto.GetInstrument;

namespace DeviceHub.Lis.Impl
{
    public class LisClient : ILisClient
    {
        private readonly string logType = nameof(LisClient);
        private static readonly LisClient _instance = new();
        public static LisClient Instance => _instance;

        private LisClient()
        {
        }

        public async Task<GetInstrument> GetInstrument(long instrumentId)
        {
            return new GetInstrument
            {
                InstrumentId = 446,
                InstrumentModel = "AUTOLAS",
                InstrumentName = "流水线Autolas",
                AuthCode = "8F4A-92C7-D1E5-B3A9",
                ExpireTime = 1782555453000,
                Status = AuthCodeStatus.Normal
            };
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
                    serialPortConfig.Encoding = "UTF-8";
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
                    tcpConfig.Encoding = "UTF-8";
                    tcpConfig.Host = "0.0.0.0";
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

        public Task<Page<GetInstrumentItemMappingPage>> GetInstrumentItemMappingPage(int instrumentId, int pageSize, int pageIndex)
        {
            const int totalCount = 1500;
            var allItems = new List<GetInstrumentItemMappingPage>
            {
                new() { InstrumentItemCode = "GLU", InstrumentItemName = "葡萄糖", LisItemCode = "L001", Unit = "mmol/L" },
                new() { InstrumentItemCode = "ALT", InstrumentItemName = "丙氨酸氨基转移酶", LisItemCode = "L002", Unit = "U/L" },
                new() { InstrumentItemCode = "AST", InstrumentItemName = "天门冬氨酸氨基转移酶", LisItemCode = "L003", Unit = "U/L" },
                new() { InstrumentItemCode = "CREA", InstrumentItemName = "肌酐", LisItemCode = "L004", Unit = "μmol/L" },
                new() { InstrumentItemCode = "UREA", InstrumentItemName = "尿素", LisItemCode = "L005", Unit = "mmol/L" },
                new() { InstrumentItemCode = "TP", InstrumentItemName = "总蛋白", LisItemCode = "L006", Unit = "g/L" },
                new() { InstrumentItemCode = "ALB", InstrumentItemName = "白蛋白", LisItemCode = "L007", Unit = "g/L" },
                new() { InstrumentItemCode = "TBIL", InstrumentItemName = "总胆红素", LisItemCode = "L008", Unit = "μmol/L" },
                new() { InstrumentItemCode = "DBIL", InstrumentItemName = "直接胆红素", LisItemCode = "L009", Unit = "μmol/L" },
                new() { InstrumentItemCode = "CHOL", InstrumentItemName = "总胆固醇", LisItemCode = "L010", Unit = "mmol/L" },
                new() { InstrumentItemCode = "TG", InstrumentItemName = "甘油三酯", LisItemCode = "L011", Unit = "mmol/L" },
                new() { InstrumentItemCode = "HDL-C", InstrumentItemName = "高密度脂蛋白胆固醇", LisItemCode = "L012", Unit = "mmol/L" },
                new() { InstrumentItemCode = "LDL-C", InstrumentItemName = "低密度脂蛋白胆固醇", LisItemCode = "L013", Unit = "mmol/L" },
                new() { InstrumentItemCode = "UA", InstrumentItemName = "尿酸", LisItemCode = "L014", Unit = "μmol/L" },
                new() { InstrumentItemCode = "K", InstrumentItemName = "钾", LisItemCode = "L015", Unit = "mmol/L" },
            };
            for (int i = 0; i < totalCount - 15; i++)
            {
                allItems.Add(new() { InstrumentItemCode = "GLU " + i, InstrumentItemName = "葡萄糖 " + i, LisItemCode = "L001 " + i, Unit = "mmol/L " + i });
            }

            int skip = Math.Max(0, (pageIndex - 1) * pageSize);
            var pageData = allItems.Skip(skip).Take(pageSize).ToList();

            return Task.FromResult(new Page<GetInstrumentItemMappingPage>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = pageData
            });
        }

        public Task<Resp<UploadSpecimenTestResultOutput>> UploadSpecimenTestResult(UploadSpecimenTestResultInput uploadSpecimenTestResultInput)
        {
            Logger.Debug(logType, $"准备上传LIS检验结果 {JsonSerializer.Serialize(uploadSpecimenTestResultInput)}");

            string resultId = Guid.NewGuid().ToString(); // 后续调用LIS接口替换，作为 external_no 回写

            Logger.Debug(logType, $"已上传LIS检验结果 resultId={resultId}");
            return Task.FromResult(Resp<UploadSpecimenTestResultOutput>.Ok(new UploadSpecimenTestResultOutput { ResultId = resultId }));
        }

        public async Task<GetSampleApplyItemOutput> GetSampleApplyItem(GetSampleApplyItemInput getSampleApplyItemInput)
        {
            // TODO 待实现
            return new GetSampleApplyItemOutput();
        }

        public async Task<List<GetSampleApplyListOutput>> GetSampleApplyList(long instrumentId, long lastId, int pageSize)
        {
            // TODO 待实现
            return [];
        }
    }
}

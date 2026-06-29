using DeviceHub.Abstractions.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Model.Vo;
using DeviceHub.Repository.Repositories;
using DeviceHub.Utils;

namespace DeviceHub.Service;

public class ClientLogService
{
    private static readonly ClientLogService _instance = new();
    public static ClientLogService Instance => _instance;

    private readonly ClientLogRepository clientLogRepository = ClientLogRepository.Instance;

    private ClientLogService()
    {
    }

    public async Task<Page<ClientLogPageItem>> GetPage(ClientLog.LevelEnum? level, ClientLog.TypeEnum? type,
        string message, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex)
    {
        int totalCount = await clientLogRepository.findCount(level, type, message, createTimeStart, createTimeEnd);
        List<ClientLog> clientLogList = await clientLogRepository.findPageDesc(level, type, message, createTimeStart, createTimeEnd, pageSize, pageIndex);

        List<ClientLogPageItem> outputList = new(clientLogList.Count);
        foreach (ClientLog row in clientLogList)
        {
            outputList.Add(new()
            {
                LevelName = row.Level.GetDescription(),
                TypeName = row.Type.GetDescription(),
                Message = row.Message,
                CreateTime = DateUtils.FormatTime(row.CreateTime)
            });
        }

        return new Page<ClientLogPageItem>
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            TotalCount = totalCount,
            Data = outputList
        };
    }
}

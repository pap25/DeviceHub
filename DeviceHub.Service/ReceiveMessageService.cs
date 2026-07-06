using DeviceHub.Abstractions.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Model.view;
using DeviceHub.Model.Vo;
using DeviceHub.Repository;
using DeviceHub.Repository.Repositories;
using DeviceHub.Utils;
using System.Text;

namespace DeviceHub.Service;

public class ReceiveMessageService
{
    private static readonly ReceiveMessageService _instance = new();
    public static ReceiveMessageService Instance => _instance;

    private readonly ReceiveMessageRepository receiveMessageRepository = ReceiveMessageRepository.Instance;
    private readonly ReceiveMessageLargeRepository receiveMessageLargeRepository = ReceiveMessageLargeRepository.Instance;
    private readonly ReceiveMessageDecodeRepository receiveMessageDecodeRepository = ReceiveMessageDecodeRepository.Instance;

    private ReceiveMessageService()
    {
    }

    public async Task<Page<ReceiveMessagePageItem>> GetPage(long instrumentId, ReceiveMessage.StatusEnum? status, ReceiveMessageDecode.TypeEnum? type,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex)
    {
        int totalCount = await receiveMessageRepository.findCount(instrumentId, status, type, barcode, sampleNo, createTimeStart, createTimeEnd);
        List<ReceiveMessageView> receiveMessageList = await receiveMessageRepository.findPageDesc(instrumentId, status, type, barcode, sampleNo, createTimeStart, createTimeEnd, pageSize, pageIndex);

        List<ReceiveMessagePageItem> outputList = new(receiveMessageList.Count);
        foreach (ReceiveMessageView row in receiveMessageList)
        {
            outputList.Add(new()
            {
                StatusName = row.Status.GetDescription(),
                TypeName = row.Type == null ? string.Empty : row.Type.GetDescription(),
                RawMessage = Encoding.UTF8.GetString(row.RawMessage),
                DecodeResult = row.ResultJson,
                Barcode = row.Barcode,
                SampleNo = row.SampleNo,
                CreateTime = DateUtils.FormatTime(row.CreateTime),
                ErrorMessage = row.ErrorMessage
            });
        }

        return new Page<ReceiveMessagePageItem>
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            TotalCount = totalCount,
            Data = outputList
        };
    }

    public async Task Save(long instrumentId, byte[] rawMessage)
    {
        // 添加队列表 receive_message、receive_message_large

        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        ReceiveMessage receiveMessage = new()
        {
            InstrumentId = instrumentId,
            Status = ReceiveMessage.StatusEnum.Pending,
            ErrorMessage = "",
            CreateTime = now,
            UpdateTime = now
        };
        ReceiveMessageLarge receiveMessageLarge = new()
        {
            RawMessage = rawMessage
        };

        await DbHelper.ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            long receiveMessageId = await receiveMessageRepository.Insert(receiveMessage, connection, transaction);
            receiveMessageLarge.ReceiveMessageId = receiveMessageId;
            await receiveMessageLargeRepository.Insert(receiveMessageLarge, connection, transaction);
        });
    }

    public void UpdateSuccess(long id, ReceiveMessageDecode.TypeEnum type, string externalNo, string sampleNo, string barcode, string resultJson)
    {
        // 更新 receive_message 新增 receive_message_decode

        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        ReceiveMessageDecode receiveMessageDecode = new()
        {
            ReceiveMessageId = id,
            ExternalNo = externalNo,
            Type = type,
            SampleNo = sampleNo,
            Barcode = barcode,
            ResultJson = resultJson,
            CreateTime = now,
            UpdateTime = now
        };

        receiveMessageRepository.UpdateStatusAndUpdateTimeById(id, ReceiveMessage.StatusEnum.Success, now);
        receiveMessageDecodeRepository.Insert(receiveMessageDecode);
    }
}
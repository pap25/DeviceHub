using DeviceHub.Abstractions.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Model.view;
using DeviceHub.Model.Vo;
using DeviceHub.Repository;
using DeviceHub.Repository.Repositories;
using DeviceHub.Utils;

namespace DeviceHub.Service;

public class SendMessageService
{
    private static readonly SendMessageService _instance = new();
    public static SendMessageService Instance => _instance;

    private readonly SendMessageRepository sendMessageRepository = SendMessageRepository.Instance;
    private readonly SendMessageEncoderRepository sendMessageEncoderRepository = SendMessageEncoderRepository.Instance;

    private SendMessageService()
    {
    }

    public async Task<Page<SendMessagePageItem>> GetPage(long instrumentId, SendMessage.StatusEnum? status,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex)
    {
        int totalCount = await sendMessageRepository.findCount(instrumentId, status, barcode, sampleNo, createTimeStart, createTimeEnd);
        List<SendMessageView> sendMessageList = await sendMessageRepository.findPageDesc(instrumentId, status, barcode, sampleNo, createTimeStart, createTimeEnd, pageSize, pageIndex);

        List<SendMessagePageItem> outputList = new(sendMessageList.Count);
        foreach (SendMessageView row in sendMessageList)
        {
            outputList.Add(new()
            {
                StatusName = row.Status.GetDescription(),
                SendJson = row.SendJson,
                SendContent = row.SendContent,
                Barcode = row.Barcode,
                SampleNo = row.SampleNo,
                CreateTime = DateUtils.FormatTime(row.CreateTime),
                ErrorMessage = row.ErrorMessage
            });
        }

        return new Page<SendMessagePageItem>
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            TotalCount = totalCount,
            Data = outputList
        };
    }

    public async Task UpdateSuccessRequestApplication(long sendMessageId, byte[] sendContent)
    {
        // 保存 send_message_encoder 更新 send_message
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        SendMessageEncoder sendMessageEncoder = new()
        {
            SendMessageId = sendMessageId,
            SendContent = sendContent,
            CreateTime = now,
            UpdateTime = now
        };

        await DbHelper.ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            await sendMessageRepository.UpdateStatusAndUpdateTimeById(
                sendMessageId, SendMessage.StatusEnum.Success, now, connection, transaction);
            await sendMessageEncoderRepository.Insert(sendMessageEncoder, connection, transaction);
        });
    }

    public void SaveIssueApplication(long lastId, long instrumentId, string externalNo, string sampleNo, string barcode, string sendJson)
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        SendMessage sendMessage = new()
        {
            InstrumentId = instrumentId,
            Type = SendMessage.TypeEnum.IssueApplication,
            ExternalNo = externalNo,
            SampleNo = sampleNo,
            Barcode = barcode,
            Status = SendMessage.StatusEnum.Pending,
            ErrorMessage = string.Empty,
            CreateTime = now,
            UpdateTime = now
        };
        SendMessageLarge receiveMessageLarge = new()
        {
            SendJson = sendJson
        };
    }
}

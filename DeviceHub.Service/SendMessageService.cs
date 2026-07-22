using DeviceHub.Abstractions.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Model.view;
using DeviceHub.Model.Vo;
using DeviceHub.Repository;
using DeviceHub.Repository.Repositories;
using DeviceHub.Utils;
using System.Text;

namespace DeviceHub.Service;

public class SendMessageService
{
    private static readonly SendMessageService _instance = new();
    public static SendMessageService Instance => _instance;

    private readonly ISendMessageRepository sendMessageRepository = SendMessageRepository.Instance;
    private readonly ISendMessageEncoderRepository sendMessageEncoderRepository = SendMessageEncoderRepository.Instance;
    private readonly ISendMessageLargeRepository sendMessageLargeRepository = SendMessageLargeRepository.Instance;
    private readonly IDictionaryRepository dictionaryRepository = DictionaryRepository.Instance;

    private SendMessageService()
    {
    }

    public async Task<Page<SendMessagePageItem>> GetPage(long instrumentId, Encoding encoding,
        SendMessage.StatusEnum? status, SendMessage.TypeEnum? type,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex)
    {
        int totalCount = await sendMessageRepository.findCount(instrumentId, status, type, barcode, sampleNo, createTimeStart, createTimeEnd);
        List<SendMessageView> sendMessageList = await sendMessageRepository.findPageDesc(instrumentId, status, type, barcode, sampleNo, createTimeStart, createTimeEnd, pageSize, pageIndex);
        List<SendMessagePageItem> outputList = new(sendMessageList.Count);
        foreach (SendMessageView row in sendMessageList)
        {
            outputList.Add(new()
            {
                Id = row.Id,
                StatusName = row.Status.GetDescription(),
                SendJson = row.SendJson,
                SendContent = encoding.GetString(row.SendContent),
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

        SendMessageLarge sendMessageLarge = new()
        {
            SendJson = sendJson
        };

        DbHelper.ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            long sendMessageId = await sendMessageRepository.Insert(sendMessage, connection, transaction);

            sendMessageLarge.SendMessageId = sendMessageId;
            await sendMessageLargeRepository.Insert(sendMessageLarge, connection, transaction);

            await dictionaryRepository.UpsertValue(DataDictionary.Keys.LisIssueApplicationLastId, lastId.ToString(), connection, transaction);
        }).GetAwaiter().GetResult();
    }
}
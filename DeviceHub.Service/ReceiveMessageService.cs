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

    private readonly IReceiveMessageRepository receiveMessageRepository = ReceiveMessageRepository.Instance;
    private readonly IReceiveMessageLargeRepository receiveMessageLargeRepository = ReceiveMessageLargeRepository.Instance;
    private readonly IReceiveMessageDecodeRepository receiveMessageDecodeRepository = ReceiveMessageDecodeRepository.Instance;
    private readonly ISendMessageRepository sendMessageRepository = SendMessageRepository.Instance;
    private readonly ISendMessageLargeRepository sendMessageLargeRepository = SendMessageLargeRepository.Instance;

    private ReceiveMessageService()
    {
    }

    public async Task<Page<ReceiveMessagePageItem>> GetPage(long instrumentId, Encoding encoding,
        ReceiveMessage.StatusEnum? status, ReceiveMessageDecode.TypeEnum? type,
        string barcode, string sampleNo, long createTimeStart, long createTimeEnd, int pageSize, int pageIndex)
    {
        int totalCount = await receiveMessageRepository.findCount(instrumentId, status, type, barcode, sampleNo, createTimeStart, createTimeEnd);
        List<ReceiveMessageView> receiveMessageList = await receiveMessageRepository.findPageDesc(instrumentId, status, type, barcode, sampleNo, createTimeStart, createTimeEnd, pageSize, pageIndex);
        List<ReceiveMessagePageItem> outputList = new(receiveMessageList.Count);
        foreach (ReceiveMessageView row in receiveMessageList)
        {
            outputList.Add(new()
            {
                Id = row.Id,
                StatusName = row.Status.GetDescription(),
                TypeName = row.Type == null ? string.Empty : row.Type.GetDescription(),
                RawMessage = encoding.GetString(row.RawMessage),
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

    public async Task UpdateStatusToPending(IReadOnlyList<long> ids)
    {
        if (ids.Count == 0)
        {
            return;
        }

        await receiveMessageRepository.UpdateStatusAndErrorMessageAndUpdateTimeByIds(
            ids,
            ReceiveMessage.StatusEnum.Pending,
            string.Empty,
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    public async Task UpdateSuccessTestResult(long id, string externalNo, string sampleNo, string barcode, string resultJson)
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        ReceiveMessageDecode receiveMessageDecode = new()
        {
            ReceiveMessageId = id,
            ExternalNo = externalNo,
            Type = ReceiveMessageDecode.TypeEnum.TestResult,
            SampleNo = sampleNo,
            Barcode = barcode,
            ResultJson = resultJson,
            CreateTime = now,
            UpdateTime = now
        };

        await DbHelper.ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            await receiveMessageRepository.UpdateStatusAndUpdateTimeById(id, ReceiveMessage.StatusEnum.Success, now, connection, transaction);
            await receiveMessageDecodeRepository.InsertForUpdateByReceiveMessageId(receiveMessageDecode, connection, transaction);
        });
    }

    public void SaveSampleQuery(long instrumentId, long receiveMessageId, string sampleNo, string barcode, string decodeResultJson, string sendJson)
    {
        string externalNo = SendMessage.RequestApplicationPrefix + receiveMessageId;
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        ReceiveMessageDecode receiveMessageDecode = new()
        {
            ReceiveMessageId = receiveMessageId,
            ExternalNo = externalNo,
            Type = ReceiveMessageDecode.TypeEnum.SampleQuery,
            SampleNo = sampleNo,
            Barcode = barcode,
            ResultJson = decodeResultJson,
            CreateTime = now,
            UpdateTime = now
        };

        SendMessage sendMessage = new()
        {
            InstrumentId = instrumentId,
            Type = SendMessage.TypeEnum.RequestApplication,
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
            SendJson = sendJson,
        };

        DbHelper.ExecuteInTransactionAsync(async (connection, transaction) =>
        {
            await receiveMessageRepository.UpdateStatusAndUpdateTimeById(receiveMessageId, ReceiveMessage.StatusEnum.Success, now, connection, transaction);
            await receiveMessageDecodeRepository.InsertForUpdateByReceiveMessageId(receiveMessageDecode, connection, transaction);
            long sendMessageId = await sendMessageRepository.InsertForUpdateByExternalNo(sendMessage, connection, transaction);

            sendMessageLarge.SendMessageId = sendMessageId;
            await sendMessageLargeRepository.InsertForUpdateBySendMessageId(sendMessageLarge, connection, transaction);
        }).GetAwaiter().GetResult();
    }
}
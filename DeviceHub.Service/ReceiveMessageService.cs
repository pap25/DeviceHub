using DeviceHub.Abstractions.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Model.view;
using DeviceHub.Model.Vo;
using DeviceHub.Repository.Repositories;
using DeviceHub.Utils;
using SQLitePCL;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace DeviceHub.Service;

public class ReceiveMessageService
{
    private static readonly ReceiveMessageService _instance = new();
    public static ReceiveMessageService Instance => _instance;

    private readonly ReceiveMessageRepository receiveMessageRepository = ReceiveMessageRepository.Instance;

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
                RawMessage = row.RawMessage,
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
}
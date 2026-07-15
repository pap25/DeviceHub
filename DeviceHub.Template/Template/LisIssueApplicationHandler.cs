using DeviceHub.Utils;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Service;
using System.Text.Json;

namespace DeviceHub.Template.Template
{
    public class LisIssueApplicationHandler : IBatchTaskHandler<GetSampleApplyListOutput>
    {
        private readonly string logType = nameof(LisIssueApplicationHandler);
        private long _instrumentId;
        private readonly ILisClient lisClient = LisClient.Instance;
        private readonly SendMessageService sendMessageService = SendMessageService.Instance;
        private readonly DictionaryRepository dictionaryRepository = DictionaryRepository.Instance;
        private readonly IConsumeTask? sendHandlerTask;

        public LisIssueApplicationHandler(long instrumentId, IConsumeTask sendHandlerTask)
        {
            this._instrumentId = instrumentId;
            this.sendHandlerTask = sendHandlerTask;
        }

        public IEnumerable<GetSampleApplyListOutput> SearchTask()
        {
            try
            {
                long lastId = 0;
                string? lastIdValue = dictionaryRepository.GetValueByCkey(DataDictionary.Keys.LisIssueApplicationLastId).GetAwaiter().GetResult();
                if (lastIdValue != null)
                {
                    lastId = long.Parse(lastIdValue);
                }
                else
                {
                    dictionaryRepository.UpsertValue(DataDictionary.Keys.LisIssueApplicationLastId, "0").GetAwaiter().GetResult();
                }

                List<GetSampleApplyListOutput> list = lisClient.GetSampleApplyList(_instrumentId, lastId, 20).GetAwaiter().GetResult();
                if (list.Count > 0)
                    Logger.Debug(logType, $"LIS拉取下单申请条数:{list.Count}");
                return list;
            }
            catch (Exception ex)
            {
                Logger.Warn(logType, $"到LIS拉取下单申请异常 instrumentId={_instrumentId}: {ex.Message}");
                return [];
            }
        }

        public void HandleTask(GetSampleApplyListOutput task)
        {
            try
            {
                sendMessageService.SaveIssueApplication(
                    task.Id,
                    _instrumentId,
                    task.Id.ToString(),
                    task.SampleNo ?? string.Empty,
                    task.Barcode ?? string.Empty,
                    JsonSerializer.Serialize(task));
            }
            catch (Exception ex)
            {
                Logger.Warn(logType, $"保存LIS拉取下单申请异常 id={task.Id}: {ex.Message}");
            }

            sendHandlerTask?.NotifyConsume();
        }
    }
}

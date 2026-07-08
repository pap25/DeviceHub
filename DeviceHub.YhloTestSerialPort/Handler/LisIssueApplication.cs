using DeviceHub.Abstractions;
using DeviceHub.Base.Common;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Model.Entities;
using DeviceHub.Repository.Repositories;
using DeviceHub.Service;
using System.Text.Json;

namespace DeviceHub.YhloTestSerialPort.Handler
{
    public class LisIssueApplication : IBatchTaskHandler<GetSampleApplyListOutput>
    {
        private readonly string logType = nameof(LisIssueApplication);
        private long _instrumentId;
        private readonly ILisClient lisClient = LisClient.Instance;
        private readonly SendMessageService sendMessageService = SendMessageService.Instance;
        private readonly DictionaryRepository dictionaryRepository = DictionaryRepository.Instance;

        public LisIssueApplication(long instrumentId)
        {
            _instrumentId = instrumentId;
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

                return lisClient.GetSampleApplyList(_instrumentId, lastId, 20).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Warn(logType, $"SearchTask异常 instrumentId={_instrumentId}: {ex.Message}");
                return [];
            }
        }

        public void HandleTask(GetSampleApplyListOutput task)
        {
            try
            {
                sendMessageService.SaveIssueApplication(task.Id, _instrumentId, "", "", "", JsonSerializer.Serialize(task));
            }
            catch (Exception ex)
            {
                Logger.Warn(logType, $"HandleTask异常 id={task.Id}: {ex.Message}");
            }
        }
    }
}

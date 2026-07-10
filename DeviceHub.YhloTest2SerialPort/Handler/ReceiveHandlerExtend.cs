using DeviceHub.Abstractions.Dto;
using DeviceHub.Utils;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Model.Entities;
using DeviceHub.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using static DeviceHub.YhloTest2SerialPort.Protocol.AstmMessageDecode;
using static DeviceHub.YhloTest2SerialPort.Protocol.AstmMessageEntity;

namespace DeviceHub.YhloTest2SerialPort.Handler
{
    public partial class ReceiveHandler
    {
        public void UploadSpecimenTestResult(ReceiveMessage task, ParseResult parseResult)
        {
            if (parseResult.TestOrderRecord == null)
            {
                MarkFailed(task.Id, "数据异常没有订单记录", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                return;
            }

            // 检验结果 1 上传到LIS 2 更新状态并记录
            UploadSpecimenTestResultInput uploadSpecimenTestResultInput = ToUploadSpecimenTestResultInput(parseResult);
            Resp<UploadSpecimenTestResultOutput> resp = lisClient.UploadSpecimenTestResult(uploadSpecimenTestResultInput).GetAwaiter().GetResult();
            if (!resp.IsSuccess())
            {
                MarkFailed(task.Id, resp.GetErrorMsg() ?? "", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                return;
            }
            string resultId = resp.GetData().ResultId;

            receiveMessageService.UpdateSuccessTestResult(task.Id, resultId, parseResult.TestOrderRecord.SampleId,
                parseResult.TestOrderRecord.InstrumentSpecimenId, JsonSerializer.Serialize(uploadSpecimenTestResultInput)).GetAwaiter();
        }

        private UploadSpecimenTestResultInput ToUploadSpecimenTestResultInput(ParseResult parseResult)
        {
            // TODO
            return new UploadSpecimenTestResultInput();
        }

        public void SaveSampleQuery(ReceiveMessage task, RequestInformationRecord requestInformationRecord)
        {
            string sampleNo = string.IsNullOrEmpty(requestInformationRecord.PatientId) ? requestInformationRecord.EndingRangeId : requestInformationRecord.PatientId;
            string barcode = requestInformationRecord.SpecimenId;

            /** 1 到LIS接口查询检验信息
             *  2 构建Send对象
             *  3 新增 receive_message_decode send_message send_message_large
             * **/
            GetSampleApplyItemInput getSampleApplyItemInput = new()
            {
                SampleNo = sampleNo,
                Barcode = barcode,
            };
            GetSampleApplyItemOutput getSampleApplyItemOutput = lisClient.GetSampleApplyItem(getSampleApplyItemInput).GetAwaiter().GetResult();

            receiveMessageService.SaveSampleQuery(_instrumentId, task.Id, sampleNo, barcode, JsonSerializer.Serialize(getSampleApplyItemInput), JsonSerializer.Serialize(getSampleApplyItemOutput));
        }
    }
}

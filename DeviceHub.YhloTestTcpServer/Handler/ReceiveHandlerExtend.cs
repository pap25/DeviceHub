using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using System.Text.Json;
using static DeviceHub.YhloTestTcpServer.Protocol.Hl7MessageDecode;
using static DeviceHub.YhloTestTcpServer.Protocol.Hl7MessageEntity;

namespace DeviceHub.YhloTestTcpServer.Handler
{
    public partial class ReceiveHandler
    {
        public void UploadSpecimenTestResult(ReceiveMessage task, ParseResult parseResult)
        {
            ObrSegment? obrSegment = parseResult.FirstObrSegment;
            if (obrSegment == null)
            {
                MarkFailed(task.Id, "数据异常没有OBR段");
                return;
            }

            if (parseResult.ObxSegmentList.Count == 0)
            {
                MarkFailed(task.Id, "数据异常没有OBX段");
                return;
            }

            UploadSpecimenTestResultInput uploadSpecimenTestResultInput = ToUploadSpecimenTestResultInput(parseResult);
            Resp<UploadSpecimenTestResultOutput> resp = lisClient.UploadSpecimenTestResult(uploadSpecimenTestResultInput).GetAwaiter().GetResult();
            if (!resp.IsSuccess())
            {
                MarkFailed(task.Id, resp.GetErrorMsg() ?? "");
                return;
            }

            string resultId = resp.GetData().ResultId;
            receiveMessageService.UpdateSuccessTestResult(task.Id, resultId, uploadSpecimenTestResultInput.SampleNo,
                uploadSpecimenTestResultInput.Barcode, JsonSerializer.Serialize(uploadSpecimenTestResultInput)).GetAwaiter();
        }

        private UploadSpecimenTestResultInput ToUploadSpecimenTestResultInput(ParseResult parseResult)
        {
            ObrSegment obrSegment = parseResult.FirstObrSegment!;
            string sampleNo = obrSegment.FillerOrderNumber;
            string barcode = string.IsNullOrEmpty(obrSegment.PlacerOrderNumber)
                ? sampleNo
                : obrSegment.PlacerOrderNumber;
            string defaultTestTime = obrSegment.ObservationDateTime;

            return new UploadSpecimenTestResultInput
            {
                InstrumentId = _instrumentId,
                SampleNo = sampleNo,
                Barcode = barcode,
                Items = parseResult.ObxSegmentList.Select(obx => new TestResultItem
                {
                    ItemCode = obx.ObservationIdentifier,
                    ItemName = obx.ObservationSubId,
                    ResultValue = string.IsNullOrEmpty(obx.ObservationValue) ? obx.Probability : obx.ObservationValue,
                    Unit = obx.Units,
                    AbnormalFlag = obx.AbnormalFlags,
                    TestTime = string.IsNullOrEmpty(obx.ObservationDateTime) ? defaultTestTime : obx.ObservationDateTime
                }).ToList()
            };
        }

        public void SaveSampleQuery(ReceiveMessage task, ParseResult parseResult)
        {
            string sampleNo = string.Empty;
            string barcode = string.Empty;

            if (parseResult.QrdSegment != null && !string.IsNullOrEmpty(parseResult.QrdSegment.SubjectFilter))
            {
                barcode = parseResult.QrdSegment.SubjectFilter;
            }

            if (parseResult.QrfSegment != null && !string.IsNullOrEmpty(parseResult.QrfSegment.SampleStartNo))
            {
                sampleNo = parseResult.QrfSegment.SampleStartNo;
                if (string.IsNullOrEmpty(barcode))
                    barcode = string.IsNullOrEmpty(parseResult.QrfSegment.SampleEndNo)
                        ? sampleNo
                        : parseResult.QrfSegment.SampleEndNo;
            }
            else if (!string.IsNullOrEmpty(barcode))
            {
                sampleNo = barcode;
            }

            if (string.IsNullOrEmpty(sampleNo) && string.IsNullOrEmpty(barcode))
            {
                MarkFailed(task.Id, "数据异常没有样本信息");
                return;
            }

            GetSampleApplyItemInput getSampleApplyItemInput = new()
            {
                SampleNo = sampleNo,
                Barcode = barcode,
            };
            GetSampleApplyItemOutput getSampleApplyItemOutput = lisClient.GetSampleApplyItem(getSampleApplyItemInput).GetAwaiter().GetResult();

            receiveMessageService.SaveSampleQuery(_instrumentId, task.Id, sampleNo, barcode,
                JsonSerializer.Serialize(getSampleApplyItemInput), JsonSerializer.Serialize(getSampleApplyItemOutput));
        }
    }
}

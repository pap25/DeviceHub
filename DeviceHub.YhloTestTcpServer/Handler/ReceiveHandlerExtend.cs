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
            if (parseResult.ObrSegmentList.Count == 0)
            {
                MarkFailed(task.Id, "数据异常没有OBR段");
                return;
            }

            if (!parseResult.AllObxSegments.Any())
            {
                MarkFailed(task.Id, "数据异常没有OBX段");
                return;
            }

            // 每个 OBR 组（含其下 OBX）按样本分别上传；同条码的 OBR 结果合并为一次上传
            var groups = parseResult.ObrSegmentList
                .Where(obr => obr.ObxSegmentList.Count > 0)
                .GroupBy(obr =>
                {
                    string sampleNo = obr.FillerOrderNumber;
                    string barcode = string.IsNullOrEmpty(obr.PlacerOrderNumber) ? sampleNo : obr.PlacerOrderNumber;
                    return $"{sampleNo}\0{barcode}";
                });

            var resultIdList = new List<string>();
            var sampleNoList = new List<string>();
            var barcodeList = new List<string>();
            var inputList = new List<UploadSpecimenTestResultInput>();

            foreach (var group in groups)
            {
                UploadSpecimenTestResultInput input = ToUploadSpecimenTestResultInput(group.ToList());
                Resp<UploadSpecimenTestResultOutput> resp = lisClient.UploadSpecimenTestResult(input).GetAwaiter().GetResult();
                if (!resp.IsSuccess())
                {
                    MarkFailed(task.Id, resp.GetErrorMsg() ?? "");
                    return;
                }

                resultIdList.Add(resp.GetData().ResultId);
                sampleNoList.Add(input.SampleNo);
                barcodeList.Add(input.Barcode);
                inputList.Add(input);
            }

            receiveMessageService.UpdateSuccessTestResult(
                task.Id,
                string.Join(",", resultIdList),
                string.Join(",", sampleNoList),
                string.Join(",", barcodeList),
                inputList.Count == 1 ? JsonSerializer.Serialize(inputList[0]) : JsonSerializer.Serialize(inputList)).GetAwaiter();
        }

        private UploadSpecimenTestResultInput ToUploadSpecimenTestResultInput(List<ObrSegment> obrGroup)
        {
            ObrSegment first = obrGroup[0];
            string sampleNo = first.FillerOrderNumber;
            string barcode = string.IsNullOrEmpty(first.PlacerOrderNumber)
                ? sampleNo
                : first.PlacerOrderNumber;

            var items = new List<TestResultItem>();
            foreach (ObrSegment obr in obrGroup)
            {
                string defaultTestTime = obr.ObservationDateTime;
                foreach (ObxSegment obx in obr.ObxSegmentList)
                {
                    items.Add(new TestResultItem
                    {
                        ItemCode = obx.ObservationIdentifier,
                        ItemName = obx.ObservationSubId,
                        ResultValue = string.IsNullOrEmpty(obx.ObservationValue) ? obx.Probability : obx.ObservationValue,
                        Unit = obx.Units,
                        AbnormalFlag = obx.AbnormalFlags,
                        TestTime = string.IsNullOrEmpty(obx.ObservationDateTime) ? defaultTestTime : obx.ObservationDateTime
                    });
                }
            }

            return new UploadSpecimenTestResultInput
            {
                InstrumentId = _instrumentId,
                SampleNo = sampleNo,
                Barcode = barcode,
                Items = items
            };
        }

        public void UploadQualityControlTestResult(ReceiveMessage task, ParseResult parseResult)
        {
            if (parseResult.ObrSegmentList.Count == 0)
            {
                MarkFailed(task.Id, "数据异常没有OBR段");
                return;
            }

            // 质控：每个 OBR 对应一个检验项目，消息中可有多个 OBR
            var resultIdList = new List<string>();
            var sampleNoList = new List<string>();
            var barcodeList = new List<string>();
            var inputList = new List<UploadQualityControlTestResultInput>(1);

            foreach (ObrSegment obr in parseResult.ObrSegmentList)
            {
                UploadQualityControlTestResultInput input = ToUploadQualityControlTestResultInput(obr);
                if (input.Items.Count == 0)
                    continue;

                Resp<UploadQualityControlTestResultOutput> resp = lisClient.UploadQualityControlTestResult(input).GetAwaiter().GetResult();
                if (!resp.IsSuccess())
                {
                    MarkFailed(task.Id, resp.GetErrorMsg() ?? "");
                    return;
                }

                string barcode = input.Items.Select(i => i.QcBarcode).FirstOrDefault(b => !string.IsNullOrEmpty(b)) ?? input.ItemCode;
                resultIdList.Add(resp.GetData().ResultId);
                sampleNoList.Add(input.ItemCode);
                barcodeList.Add(barcode);
                inputList.Add(input);
            }

            if (inputList.Count == 0)
            {
                MarkFailed(task.Id, "数据异常没有质控结果");
                return;
            }

            receiveMessageService.UpdateSuccessTestResult(
                task.Id,
                string.Join(",", resultIdList),
                string.Join(",", sampleNoList),
                string.Join(",", barcodeList),
                inputList.Count == 1 ? JsonSerializer.Serialize(inputList[0]) : JsonSerializer.Serialize(inputList)).GetAwaiter();
        }

        private UploadQualityControlTestResultInput ToUploadQualityControlTestResultInput(ObrSegment obr)
        {
            string[] qcNos = SplitComponent(obr.DangerCode);
            string[] qcNames = SplitComponent(obr.RelevantClinicalInfo);
            string[] lotNos = SplitComponent(obr.SpecimenReceivedDateTime);
            string[] expiryDates = SplitComponent(obr.SpecimenSource);
            string[] levels = SplitComponent(obr.OrderCallbackPhoneNumber);
            string[] means = SplitComponent(obr.PlacerField1);
            string[] stdDevs = SplitComponent(obr.PlacerField2);
            string[] resultValues = SplitComponent(obr.FillerField1);
            string[] qcBarcodes = SplitComponent(obr.QcBarCode);

            int count = 0;
            if (int.TryParse(obr.SpecimenActionCode, out int qcCount) && qcCount > 0)
                count = qcCount;

            count = Math.Max(count, qcNos.Length);
            count = Math.Max(count, qcNames.Length);
            count = Math.Max(count, lotNos.Length);
            count = Math.Max(count, expiryDates.Length);
            count = Math.Max(count, levels.Length);
            count = Math.Max(count, means.Length);
            count = Math.Max(count, stdDevs.Length);
            count = Math.Max(count, resultValues.Length);

            var items = new List<QualityControlResultItem>(count);
            for (int i = 0; i < count; i++)
            {
                items.Add(new QualityControlResultItem
                {
                    QcNo = GetComponent(qcNos, i),
                    QcName = GetComponent(qcNames, i),
                    LotNo = GetComponent(lotNos, i),
                    ExpiryDate = GetComponent(expiryDates, i),
                    Level = GetComponent(levels, i),
                    Mean = GetComponent(means, i),
                    StdDev = GetComponent(stdDevs, i),
                    ResultValue = GetComponent(resultValues, i),
                    QcBarcode = qcBarcodes.Length == 1
                        ? qcBarcodes[0]
                        : GetComponent(qcBarcodes, i)
                });
            }

            return new UploadQualityControlTestResultInput
            {
                InstrumentId = _instrumentId,
                ItemCode = obr.PlacerOrderNumber,
                ItemName = obr.FillerOrderNumber,
                TestTime = obr.ObservationDateTime,
                QcRule = obr.CollectionVolume,
                ModuleId = obr.OrderingProvider,
                TraceabilityInfo = obr.ObservationEndDateTime,
                Items = items
            };
        }

        private static string[] SplitComponent(string value)
        {
            if (string.IsNullOrEmpty(value))
                return [];
            return value.Split('^');
        }

        private static string GetComponent(string[] components, int index)
        {
            if (index < 0 || index >= components.Length)
                return string.Empty;
            return components[index];
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

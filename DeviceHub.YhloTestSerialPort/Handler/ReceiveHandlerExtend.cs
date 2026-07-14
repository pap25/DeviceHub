using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis.Dto;
using DeviceHub.Model.Entities;
using System.Text.Json;
using static DeviceHub.YhloTestSerialPort.Protocol.AstmMessageDecode;
using static DeviceHub.YhloTestSerialPort.Protocol.AstmMessageEntity;

namespace DeviceHub.YhloTestSerialPort.Handler
{
    public partial class ReceiveHandler
    {
        public void UploadSpecimenTestResult(ReceiveMessage task, ParseResult parseResult)
        {
            if (parseResult.TestOrderRecord == null)
            {
                MarkFailed(task.Id, "数据异常没有订单记录");
                return;
            }

            // 检验结果 1 上传到LIS 2 更新状态并记录
            UploadSpecimenTestResultInput uploadSpecimenTestResultInput = ToUploadSpecimenTestResultInput(parseResult);
            Resp<UploadSpecimenTestResultOutput> resp = lisClient.UploadSpecimenTestResult(uploadSpecimenTestResultInput).GetAwaiter().GetResult();
            if (!resp.IsSuccess())
            {
                MarkFailed(task.Id, resp.GetErrorMsg() ?? "");
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

        public void UploadQualityControlTestResult(ReceiveMessage task, ParseResult parseResult)
        {
            if (parseResult.TestOrderRecord == null)
            {
                MarkFailed(task.Id, "数据异常没有订单记录");
                return;
            }

            UploadQualityControlTestResultInput input = ToUploadQualityControlTestResultInput(parseResult);
            if (input.Items.Count == 0)
            {
                MarkFailed(task.Id, "数据异常没有质控结果");
                return;
            }

            Resp<UploadQualityControlTestResultOutput> resp = lisClient.UploadQualityControlTestResult(input).GetAwaiter().GetResult();
            if (!resp.IsSuccess())
            {
                MarkFailed(task.Id, resp.GetErrorMsg() ?? "");
                return;
            }

            string resultId = resp.GetData().ResultId;
            string barcode = input.Items.Select(i => i.QcBarcode).FirstOrDefault(b => !string.IsNullOrEmpty(b)) ?? input.ItemCode;
            receiveMessageService.UpdateSuccessTestResult(task.Id, resultId, input.ItemCode, barcode, JsonSerializer.Serialize(input)).GetAwaiter();
        }

        /// <summary>
        /// ASTM 质控结果（H-12=QR）：O-5=项目代号^项目名称，O-7=质控时间，
        /// O-12=质控液编号^名称^批号^有效期^均值^浓度水平^标准差^结果值（多液用\分隔）
        /// </summary>
        private UploadQualityControlTestResultInput ToUploadQualityControlTestResultInput(ParseResult parseResult)
        {
            TestOrderRecord order = parseResult.TestOrderRecord!;
            string[] assayParts = SplitComponent(order.AssayNo);
            string[] repeats = string.IsNullOrEmpty(order.ActionCode)
                ? []
                : order.ActionCode.Split('\\', StringSplitOptions.RemoveEmptyEntries);

            var items = new List<QualityControlResultItem>(repeats.Length);
            foreach (string repeat in repeats)
            {
                string[] parts = SplitComponent(repeat);
                items.Add(new QualityControlResultItem
                {
                    QcNo = GetComponent(parts, 0),
                    QcName = GetComponent(parts, 1),
                    LotNo = GetComponent(parts, 2),
                    ExpiryDate = GetComponent(parts, 3),
                    Mean = GetComponent(parts, 4),
                    Level = GetComponent(parts, 5),
                    StdDev = GetComponent(parts, 6),
                    ResultValue = GetComponent(parts, 7)
                });
            }

            return new UploadQualityControlTestResultInput
            {
                InstrumentId = _instrumentId,
                ItemCode = GetComponent(assayParts, 0),
                ItemName = GetComponent(assayParts, 1),
                TestTime = order.RequestedDateTime,
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

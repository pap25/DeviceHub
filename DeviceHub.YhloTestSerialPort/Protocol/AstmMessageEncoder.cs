using DeviceHub.Lis.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static DeviceHub.YhloTestSerialPort.Protocol.AstmMessageDecode;

namespace DeviceHub.YhloTestSerialPort.Protocol
{
    public class AstmMessageEncoder
    {
        // 请求查询申请信息
        public static List<byte[]> EncoderRequestApplication(GetSampleApplyItemOutput sampleApplyItem)
        {
            // TODO 等待实现
            return [];
        }

        // LIS下发申请信息
        public static List<byte[]> EncoderIssueApplication(GetSampleApplyListOutput getSampleApplyListOutput)
        {
            // SampleQuery TestResult
            return [];
        }
    }
}

using DeviceHub.Lis.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static DeviceHub.Yhlo.Protocol.AstmMessageDecode;

namespace DeviceHub.YhloTestSerialPort.Protocol
{
    public class AstmMessageEncoder
    {
        // 请求查询申请信息
        public static byte[] EncoderRequestApplication(GetSampleApplyItemOutput sampleApplyItem)
        {
            return [];
        }

        // LIS下发申请信息
        public static byte[] EncoderIssueApplication(object obj)
        {
            // SampleQuery TestResult
            return [];
        }
    }
}

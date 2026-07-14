using DeviceHub.Base.Constant;
using System.Text;

namespace DeviceHub.YhloTestTcpServer.Protocol;

/// <summary>
/// HL7 MLLP 报文解析：&lt;VT&gt; segments &lt;EB&gt;&lt;CR&gt;
/// </summary>
public static class Hl7MessageVerify
{
    public sealed class VerifyParseResult
    {
        public bool Success { get; init; }

        public string ErrorMessage { get; init; } = string.Empty;

        public List<string> SegmentList { get; init; } = [];
    }

    public static VerifyParseResult VerifyParse(byte[] rawMessage)
    {
        if (rawMessage is null || rawMessage.Length == 0)
            return Fail("报文为空");

        int start = 0;
        int end = rawMessage.Length;

        if (rawMessage[start] == HL7Protocols.VT)
            start++;

        while (end > start && rawMessage[end - 1] is HL7Protocols.CR or HL7Protocols.EB)
            end--;

        if (end <= start)
            return Fail("报文内容为空");

        string text = Encoding.UTF8.GetString(rawMessage, start, end - start);
        List<string> segmentList = text
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        if (segmentList.Count == 0 || !segmentList[0].StartsWith("MSH", StringComparison.Ordinal))
            return Fail("报文缺少MSH段");

        return new VerifyParseResult
        {
            Success = true,
            SegmentList = segmentList
        };
    }

    private static VerifyParseResult Fail(string errorMessage) => new()
    {
        Success = false,
        ErrorMessage = errorMessage
    };
}

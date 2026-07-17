using DeviceHub.Template.Constant;
using DeviceHub.Utils;
using System.Text;

namespace DeviceHub.Template.Protocol;

/// <summary>
/// ASTM 多帧报文解析：<STX> FN DATA <ETB/ETX> CS <CR><LF>
/// </summary>
public static class AstmMessageVerify
{
    public sealed class VerifyParseResult
    {
        public bool Success { get; init; }

        public string ErrorMessage { get; init; } = string.Empty;

        /// <summary>
        /// 各帧 DATA 列表（不含 STX、FN、帧尾）
        /// </summary>
        public List<string> ParsedRecord { get; init; } = [];
    }

    public static VerifyParseResult VerifyParse(byte[] rawMessage, Encoding encoding)
    {
        if (rawMessage is null || rawMessage.Length == 0)
        {
            return Fail("报文为空");
        }

        byte[] buffer = rawMessage;
        if (buffer[0] != ASTMProtocols.STX)
        {
            return Fail("报文缺少STX");
        }

        List<string> parsedData = [];
        int offset = 0;

        while (offset < buffer.Length)
        {
            VerifyParseResult? frameResult = TryParseFrame(buffer, offset, encoding, out int frameLength, out string frameData);
            if (frameResult != null)
            {
                return frameResult;
            }

            parsedData.Add(frameData);
            offset += frameLength;
        }

        return new VerifyParseResult
        {
            Success = true,
            ParsedRecord = parsedData
        };
    }

    private static VerifyParseResult? TryParseFrame(
        byte[] buffer,
        int offset,
        Encoding encoding,
        out int frameLength,
        out string frameData)
    {
        frameLength = 0;
        frameData = string.Empty;

        if (buffer[offset] != ASTMProtocols.STX)
        {
            return Fail($"第{GetFrameIndex(buffer, offset)}帧缺少STX");
        }

        if (offset + 2 >= buffer.Length)
        {
            return Fail($"第{GetFrameIndex(buffer, offset)}帧不完整");
        }

        byte frameNumber = buffer[offset + 1];
        if (!IsValidFrameNumber(frameNumber))
        {
            return Fail($"第{GetFrameIndex(buffer, offset)}帧帧号非法: {(char)frameNumber}");
        }

        int payloadStart = offset + 2;
        int frameEndIndex = FindFrameEndIndex(buffer, payloadStart);
        if (frameEndIndex < 0)
        {
            return Fail($"第{GetFrameIndex(buffer, offset)}帧缺少ETX/ETB");
        }

        int frameTrailerEnd = frameEndIndex + ASTMProtocols.FrameTrailerLength;
        if (frameTrailerEnd > buffer.Length)
        {
            return Fail($"第{GetFrameIndex(buffer, offset)}帧帧尾不完整");
        }

        if (buffer[frameEndIndex + 3] != ASTMProtocols.CR || buffer[frameEndIndex + 4] != ASTMProtocols.LF)
        {
            return Fail($"第{GetFrameIndex(buffer, offset)}帧帧尾格式错误");
        }

        if (!TryParseHexByte(buffer[frameEndIndex + 1], buffer[frameEndIndex + 2], out byte expectedChecksum))
        {
            return Fail($"第{GetFrameIndex(buffer, offset)}帧校验码格式错误");
        }

        byte actualChecksum = CalculateChecksum(buffer.AsSpan(offset + 1, frameEndIndex - offset));
        if (actualChecksum != expectedChecksum)
        {
            return Fail($"第{GetFrameIndex(buffer, offset)}帧校验失败");
        }

        frameData = encoding.GetString(buffer, payloadStart, frameEndIndex - payloadStart - 1); // 把CR也去掉了
        frameLength = frameTrailerEnd - offset;
        return null;
    }

    private static int FindFrameEndIndex(byte[] buffer, int payloadStart)
    {
        for (int i = payloadStart; i < buffer.Length; i++)
        {
            if (buffer[i] == ASTMProtocols.STX)
            {
                return -1;
            }

            if (ASTMProtocols.IsFrameEnd(buffer[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static bool IsValidFrameNumber(byte frameNumber) =>
        frameNumber >= (byte)'0' && frameNumber <= (byte)'7';

    /// <summary>
    /// 校验范围：FN + DATA + ETX/ETB
    /// </summary>
    public static byte CalculateChecksum(ReadOnlySpan<byte> frameContent)
    {
        int sum = 0;
        foreach (byte value in frameContent)
        {
            sum += value;
        }

        return (byte)(sum & 0xFF);
    }

    private static bool TryParseHexByte(byte high, byte low, out byte value)
    {
        if (!TryGetHexValue(high, out int highValue) || !TryGetHexValue(low, out int lowValue))
        {
            value = 0;
            return false;
        }

        value = (byte)((highValue << 4) | lowValue);
        return true;
    }

    private static bool TryGetHexValue(byte value, out int hexValue)
    {
        if (value >= (byte)'0' && value <= (byte)'9')
        {
            hexValue = value - '0';
            return true;
        }

        if (value >= (byte)'A' && value <= (byte)'F')
        {
            hexValue = value - 'A' + 10;
            return true;
        }

        if (value >= (byte)'a' && value <= (byte)'f')
        {
            hexValue = value - 'a' + 10;
            return true;
        }

        hexValue = 0;
        return false;
    }

    private static int GetFrameIndex(byte[] buffer, int offset)
    {
        int index = 1;
        for (int i = 0; i < offset; i++)
        {
            if (buffer[i] == ASTMProtocols.STX)
            {
                index++;
            }
        }

        return index;
    }

    private static VerifyParseResult Fail(string errorMessage) => new()
    {
        Success = false,
        ErrorMessage = errorMessage
    };
}

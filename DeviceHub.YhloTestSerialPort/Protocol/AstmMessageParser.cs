using DeviceHub.Base.Constant;
using System;
using System.Text;

namespace DeviceHub.Base.Protocol
{
    /// <summary>
    /// ASTM 多帧报文解析：<STX> FN DATA <ETB/ETX> CS <CR><LF>
    /// </summary>
    public static class AstmMessageParser
    {
        public sealed class ParseResult
        {
            public bool Success { get; init; }

            public string ErrorMessage { get; init; }

            /// <summary>
            /// 各帧 DATA 拼接结果（不含 STX、FN、帧尾）
            /// </summary>
            public string ParsedData { get; init; }
        }

        public static ParseResult TryParse(string rawMessage)
        {
            if (string.IsNullOrEmpty(rawMessage))
            {
                return Fail("报文为空");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(rawMessage);
            if (buffer[0] != ASTMProtocols.STX)
            {
                return Fail("报文缺少STX");
            }

            StringBuilder parsedData = new();
            int offset = 0;

            while (offset < buffer.Length)
            {
                ParseResult? frameResult = TryParseFrame(buffer, offset, out int frameLength, out string frameData);
                if (frameResult != null)
                {
                    return frameResult;
                }

                parsedData.Append(frameData);
                offset += frameLength;
            }

            return new ParseResult
            {
                Success = true,
                ParsedData = parsedData.ToString()
            };
        }

        private static ParseResult? TryParseFrame(
            byte[] buffer,
            int offset,
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

            frameData = Encoding.UTF8.GetString(buffer, payloadStart, frameEndIndex - payloadStart);
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

            //return (byte)((256 - (sum & 0xFF)) & 0xFF);
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

        private static ParseResult Fail(string errorMessage) => new()
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}

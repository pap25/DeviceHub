using System;
using System.Collections.Generic;
using System.Text;
using static DeviceHub.Lis.Dto.GetInstrument;

namespace DeviceHub.Win.Base
{
    public class Helper
    {
        public static string MaskAuthCode(string? authCode)
        {
            if (string.IsNullOrEmpty(authCode))
                return string.Empty;

            if (authCode.Length <= 4)
                return authCode[0] + "***" + authCode[^1];

            int prefixLen = 3;
            int suffixLen = 3;
            if (authCode.Length <= prefixLen + suffixLen + 3)
            {
                prefixLen = 1;
                suffixLen = 1;
            }

            return authCode[..prefixLen] + "***" + authCode[^suffixLen..];
        }

        public static string FormatAuthStatus(AuthCodeStatus status) => status switch
        {
            AuthCodeStatus.Normal => "正常",
            AuthCodeStatus.Disabled => "停用",
            _ => status.ToString()
        };

        public static string FormatExpireTime(long expireTime)
        {
            if (expireTime <= 0)
                return "-";

            DateTimeOffset dateTime = expireTime > 9_999_999_999
                ? DateTimeOffset.FromUnixTimeMilliseconds(expireTime)
                : DateTimeOffset.FromUnixTimeSeconds(expireTime);
            return dateTime.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Utils
{
    public class DateUtils
    {
        public static string FormatTime(long time) => DateTimeOffset.FromUnixTimeMilliseconds(time).LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceHub.Base.Common
{
    public class Logger
    {
        private static readonly string LogDir = "Logs";

        public static void Info(string msg)
        {
            Directory.CreateDirectory(LogDir);

            string file = Path.Combine(
                LogDir,
                $"{DateTime.Now:yyyyMMdd}.log");

            string line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {msg}{Environment.NewLine}";

            File.AppendAllText(
                file,
                line,
                Encoding.UTF8);
        }
    }
}

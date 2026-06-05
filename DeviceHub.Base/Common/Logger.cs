using System.Text;

namespace DeviceHub.Base.Common
{
    public class Logger
    {
        private static readonly string LogDir = "Logs";

        public static void Debug(string msg) => Write("DEBUG", msg);

        public static void Info(string msg) => Write("INFO", msg);

        public static void Warn(string msg) => Write("WARN", msg);

        public static void Error(string msg) => Write("ERROR", msg);

        private static void Write(string level, string msg)
        {
            Directory.CreateDirectory(LogDir);

            string file = Path.Combine(
                LogDir,
                $"{DateTime.Now:yyyyMMdd}.log");

            string line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {msg}{Environment.NewLine}";

            File.AppendAllText(
                file,
                line,
                Encoding.UTF8);
        }

        public static void Error(string msg, Exception ex)
        {
            Directory.CreateDirectory(LogDir);

            string file = Path.Combine(
                LogDir,
                $"{DateTime.Now:yyyyMMdd}.log");

            File.AppendAllText(
                file,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] {msg}{Environment.NewLine}" +
                $"Exception: {ex}{Environment.NewLine}" +
                $"----------------------------------------{Environment.NewLine}",
                Encoding.UTF8);
        }
    }
}

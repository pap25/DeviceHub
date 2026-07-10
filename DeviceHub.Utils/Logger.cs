using System.Text;

namespace DeviceHub.Utils
{
    public class Logger
    {
        private static readonly string LogDir = "Logs";

        public static void Debug(string type, string msg) => Write("DEBUG", type, msg);

        public static void Info(string type, string msg) => Write("INFO", type, msg);

        public static void Warn(string type, string msg) => Write("WARN", type, msg);

        public static void Error(string type, string msg) => Write("ERROR", type, msg);

        private static void Write(string level, string type, string msg)
        {
            Directory.CreateDirectory(LogDir);

            string file = Path.Combine(
                LogDir,
                $"{DateTime.Now:yyyyMMdd}.log");

            string line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] [{type}] {msg}{Environment.NewLine}";

            File.AppendAllText(
                file,
                line,
                Encoding.UTF8);
        }

        public static void Error(string type, string msg, Exception ex)
        {
            Directory.CreateDirectory(LogDir);

            string file = Path.Combine(
                LogDir,
                $"{DateTime.Now:yyyyMMdd}.log");

            File.AppendAllText(
                file,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] [{type}] {msg}{Environment.NewLine}" +
                $"Exception: {ex}{Environment.NewLine}",
                //$"----------------------------------------{Environment.NewLine}",
                Encoding.UTF8);
        }
    }
}

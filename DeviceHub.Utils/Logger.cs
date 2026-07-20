using System.Text;

namespace DeviceHub.Utils
{
    public class Logger
    {
        private static readonly string LogDir = "Logs";
        private static readonly object _writeLock = new();

        public static void Debug(string type, string msg) => Write("DEBUG", type, msg);

        public static void Info(string type, string msg) => Write("INFO", type, msg);

        public static void Warn(string type, string msg) => Write("WARN", type, msg);

        public static void Error(string type, string msg) => Write("ERROR", type, msg);

        private static void Write(string level, string type, string msg)
        {
            Directory.CreateDirectory(LogDir);

            string file = Path.Combine(LogDir, $"{DateTime.Now:yyyyMMdd}.log");
            string line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] [{type}] {msg}{Environment.NewLine}";

            AppendText(file, line);
        }

        public static void Error(string type, string msg, Exception ex)
        {
            Directory.CreateDirectory(LogDir);

            string file = Path.Combine(LogDir, $"{DateTime.Now:yyyyMMdd}.log");
            string line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] [{type}] {msg}{Environment.NewLine}" +
                $"Exception: {ex}{Environment.NewLine}";

            AppendText(file, line);
        }

        private static void AppendText(string file, string text)
        {
            lock (_writeLock)
            {
                using var fs = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.Read);
                using var writer = new StreamWriter(fs, Encoding.UTF8);
                writer.Write(text);
            }
        }

        /// <summary>
        /// 读取指定日期日志文件的最后 n 行，返回结果已按时间倒序排列（最新在前）。
        /// level 不为 null 时只统计匹配该等级的日志行。
        /// date 为 null 时读取今天的日志。
        /// </summary>
        public static List<string> ReadLastLines(int n, DateTime? date = null, string? level = null)
        {
            string file = Path.Combine(LogDir, $"{(date ?? DateTime.Today):yyyyMMdd}.log");
            if (!File.Exists(file))
                return new List<string>();

            using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fs, Encoding.UTF8);

            var buffer = new Queue<string>(n + 1);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (level != null && !MatchesLevel(line, level))
                    continue;

                if (buffer.Count == n)
                    buffer.Dequeue();
                buffer.Enqueue(line);
            }

            var lines = new List<string>(buffer);
            lines.Reverse();
            return lines;
        }

        private static bool MatchesLevel(string line, string level)
        {
            // 日志格式: [时间] [等级] [类型] 内容
            return line.Contains($"] [{level}] [");
        }
    }
}

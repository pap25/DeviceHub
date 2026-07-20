using DeviceHub.Utils;

namespace DeviceHub.Win
{
    public partial class DeviceStatus
    {
        private System.Windows.Forms.Timer? _logTimer;
        private DateTime? _lastLogRefreshTime;

        private static readonly int[] LogLineOptions = { 50, 100, 200, 500, 1000 };
        private static readonly int[] LogIntervalOptions = { 3, 5, 10, 30, 60 };

        private static readonly string[] LogLevelFilterOptions = { "全部", "DEBUG", "INFO", "WARN", "ERROR" };

        private int LogLines => cboLogLines.SelectedIndex >= 0 ? LogLineOptions[cboLogLines.SelectedIndex] : LogLineOptions[1];
        private int? LogIntervalSeconds => cboLogInterval.SelectedIndex > 0 ? LogIntervalOptions[cboLogInterval.SelectedIndex - 1] : null;
        private string? LogLevelFilter => cboLogLevelFilter.SelectedIndex > 0 ? LogLevelFilterOptions[cboLogLevelFilter.SelectedIndex] : null;

        private Task initLog()
        {
            cboLogLines.Items.AddRange(LogLineOptions.Select(n => (object)$"{n}行").ToArray());
            cboLogLines.SelectedIndex = 1; // 默认 100 行

            cboLogInterval.Items.Add("不刷新");
            cboLogInterval.Items.AddRange(LogIntervalOptions.Select(s => (object)$"{s}秒").ToArray());
            cboLogInterval.SelectedIndex = 0; // 默认不自动刷新

            cboLogLevelFilter.Items.AddRange(LogLevelFilterOptions.Cast<object>().ToArray());
            cboLogLevelFilter.SelectedIndex = 0; // 默认全部

            cboLogInterval.SelectedIndexChanged += (_, _) => ApplyLogTimer();

            return Task.CompletedTask;
        }

        private Task RefreshLog()
        {
            RefreshLogContent();
            ApplyLogTimer();
            return Task.CompletedTask;
        }

        private void btnLogQuery_Click(object sender, EventArgs e)
        {
            RefreshLogContent();
        }

        private void RefreshLogContent()
        {
            var lines = Logger.ReadLastLines(LogLines, level: LogLevelFilter);
            rtbLog.Text = string.Join(Environment.NewLine, lines);
            _lastLogRefreshTime = DateTime.Now;
            UpdateLogRefreshStatus();
        }

        private void ApplyLogTimer()
        {
            if (LogIntervalSeconds == null)
            {
                StopLogTimer();
                UpdateLogRefreshStatus();
                return;
            }

            RestartLogTimer();
        }

        private void RestartLogTimer()
        {
            if (LogIntervalSeconds == null)
                return;

            _logTimer ??= new System.Windows.Forms.Timer();
            _logTimer.Tick -= LogTimer_Tick;
            _logTimer.Tick += LogTimer_Tick;
            _logTimer.Interval = LogIntervalSeconds.Value * 1000;
            _logTimer.Start();
            UpdateLogRefreshStatus();
        }

        private void UpdateLogRefreshStatus()
        {
            lblLogAutoRefresh.Text = _lastLogRefreshTime == null
                ? ""
                : $"最后刷新: {_lastLogRefreshTime:yyyy-MM-dd HH:mm:ss}";
        }

        private void StopLogTimer()
        {
            if (_logTimer == null) return;
            _logTimer.Stop();
            _logTimer.Tick -= LogTimer_Tick;
            _logTimer.Dispose();
            _logTimer = null;
        }

        private void LogTimer_Tick(object? sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != tabLog) return;
            RefreshLogContent();
        }
    }
}

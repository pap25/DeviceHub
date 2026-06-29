using DeviceHub.Abstractions.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Model.Vo;
using DeviceHub.Service;
using DeviceHub.Win.DeviceHubControl;

namespace DeviceHub.Win
{
    public partial class DeviceStatus
    {
        private async Task initLog()
        {
            BindEnumComboBox<ClientLog.LevelEnum>(cboLogLevel, true);
            BindEnumComboBox<ClientLog.TypeEnum>(cboLogType, true);
            dtpLogCreateTimeStart.Value = DateTime.Today;
            dtpLogCreateTimeEnd.Value = DateTime.Today;
        }

        private async Task RefreshLog()
        {
            await LoadLogPage(pagerLog.PageSize, pagerLog.PageIndex);
        }

        private async void btnLogQuery_Click(object sender, EventArgs e)
        {
            await LoadLogPage(pagerLog.PageSize, 1);
        }

        private async void PagerLog_PageChanged(object? sender, PagerChangedEventArgs e)
        {
            await LoadLogPage(e.PageSize, e.PageIndex);
        }

        private async Task LoadLogPage(int pageSize, int pageIndex)
        {
            ClientLog.LevelEnum? level = GetSelectedEnum<ClientLog.LevelEnum>(cboLogLevel);
            ClientLog.TypeEnum? type = GetSelectedEnum<ClientLog.TypeEnum>(cboLogType);
            string message = txtLogMessage.Text.Trim();
            long createTimeStart = new DateTimeOffset(dtpLogCreateTimeStart.Value.Date).ToUnixTimeMilliseconds();
            long createTimeEnd = new DateTimeOffset(dtpLogCreateTimeEnd.Value.Date.AddDays(1).AddMilliseconds(-1)).ToUnixTimeMilliseconds();
            Page<ClientLogPageItem> page = await ClientLogService.Instance.GetPage(level, type, message, createTimeStart, createTimeEnd, pageSize, pageIndex);
            dgvLog.DataSource = page.Data;
            pagerLog.SetPageInfo(page);
        }
    }
}

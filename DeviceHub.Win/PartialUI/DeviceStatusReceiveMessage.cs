using DeviceHub.Win.DeviceHubControl;

namespace DeviceHub.Win
{
    public partial class DeviceStatus
    {
        private async Task initReceiveMessage()
        {
            cboReceiveMessageStatus.SelectedIndex = 0;
            cboReceiveMessageType.SelectedIndex = 0;
            dtpReceiveMessageCreateTimeStart.Value = DateTime.Today;
            dtpReceiveMessageCreateTimeEnd.Value = DateTime.Today;

            await LoadReceiveMessagePageAsync(1, pagerReceiveMessage.PageSize);
        }

        private void btnReceiveMessageQuery_Click(object sender, EventArgs e)
        {

        }

        private async void PagerReceiveMessage_PageChanged(object? sender, PagerChangedEventArgs e)
        {
            await LoadReceiveMessagePageAsync(e.PageIndex, e.PageSize);
        }

        private async Task LoadReceiveMessagePageAsync(int pageIndex, int pageSize)
        {
            // TODO: 接入接收消息分页查询 API
            await Task.CompletedTask;
        }
    }
}

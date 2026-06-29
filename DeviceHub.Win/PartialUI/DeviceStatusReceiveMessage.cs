using DeviceHub.Model;
using DeviceHub.Utils;
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

            EnumExtensions.GetKeyValues<ReceiveMessage.StatusEnum>();
            EnumExtensions.GetKeyValues<ReceiveMessageDecode.TypeEnum>();

            await LoadReceiveMessagePageAsync(1, pagerReceiveMessage.PageSize);
        }

        private async Task RefreshReceiveMessageAsync()
        {
            await LoadReceiveMessagePageAsync(pagerReceiveMessage.PageIndex, pagerReceiveMessage.PageSize);
        }

        private async void btnReceiveMessageQuery_Click(object sender, EventArgs e)
        {
            await LoadReceiveMessagePageAsync(1, pagerReceiveMessage.PageSize);
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

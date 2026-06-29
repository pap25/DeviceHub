using DeviceHub.Abstractions.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Model.Vo;
using DeviceHub.Service;
using DeviceHub.Win.DeviceHubControl;

namespace DeviceHub.Win
{
    public partial class DeviceStatus
    {
        private async Task initReceiveMessage()
        {
            BindEnumComboBox<ReceiveMessage.StatusEnum>(cboReceiveMessageStatus, true);
            BindEnumComboBox<ReceiveMessageDecode.TypeEnum>(cboReceiveMessageType, true);
            cboReceiveMessageStatus.SelectedValue = ((int)ReceiveMessage.StatusEnum.Pending).ToString();
            cboReceiveMessageType.SelectedValue = ((int)ReceiveMessageDecode.TypeEnum.TestResult).ToString();
            dtpReceiveMessageCreateTimeStart.Value = DateTime.Today;
            dtpReceiveMessageCreateTimeEnd.Value = DateTime.Today;
        }

        private async Task RefreshReceiveMessage()
        {
            await LoadReceiveMessagePage(pagerReceiveMessage.PageIndex, pagerReceiveMessage.PageSize);
        }

        private async void btnReceiveMessageQuery_Click(object sender, EventArgs e)
        {
            await LoadReceiveMessagePage(1, pagerReceiveMessage.PageSize);
        }

        private async void PagerReceiveMessage_PageChanged(object? sender, PagerChangedEventArgs e)
        {
            await LoadReceiveMessagePage(e.PageIndex, e.PageSize);
        }

        private async Task LoadReceiveMessagePage(int pageIndex, int pageSize)
        {
            ReceiveMessage.StatusEnum? status = GetSelectedEnum<ReceiveMessage.StatusEnum>(cboReceiveMessageStatus);
            ReceiveMessageDecode.TypeEnum? type = GetSelectedEnum<ReceiveMessageDecode.TypeEnum>(cboReceiveMessageType);
            string barcode = txtReceiveMessageBarcode.Text.Trim();
            string sampleNo = txtReceiveMessageSampleNo.Text.Trim();
            long createTimeStart = new DateTimeOffset(dtpReceiveMessageCreateTimeStart.Value.Date).ToUnixTimeMilliseconds();
            long createTimeEnd = new DateTimeOffset(dtpReceiveMessageCreateTimeEnd.Value.Date.AddDays(1).AddMilliseconds(-1)).ToUnixTimeMilliseconds();
            Page<ReceiveMessagePageItem> page = await ReceiveMessageService.Instance.GetPage(_instrumentId, status, type, barcode, sampleNo, createTimeStart, createTimeEnd, pageSize, pageIndex);
            dgvReceiveMessage.DataSource = page.Data;
            pagerReceiveMessage.SetPageInfo(page);
        }
    }
}

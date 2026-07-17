using DeviceHub.Abstractions.Dto;
using DeviceHub.Model.Entities;
using DeviceHub.Model.Vo;
using DeviceHub.Service;
using DeviceHub.Win.DeviceHubControl;

namespace DeviceHub.Win
{
    public partial class DeviceStatus
    {
        private async Task initSendMessage()
        {
            BindEnumComboBox<SendMessage.StatusEnum>(cboSendMessageStatus, true);
            BindEnumComboBox<SendMessage.TypeEnum>(cboSendMessageType, true);
            //cboSendMessageStatus.SelectedValue = ((int)SendMessage.StatusEnum.Pending).ToString();
            dtpSendMessageCreateTimeStart.Value = DateTime.Today;
            dtpSendMessageCreateTimeEnd.Value = DateTime.Today;
        }

        private async Task RefreshSendMessage()
        {
            await LoadSendMessagePage(pagerSendMessage.PageSize, pagerSendMessage.PageIndex);
        }

        private async void btnSendMessageQuery_Click(object sender, EventArgs e)
        {
            await LoadSendMessagePage(pagerSendMessage.PageSize, 1);
        }

        private async void PagerSendMessage_PageChanged(object? sender, PagerChangedEventArgs e)
        {
            await LoadSendMessagePage(e.PageSize, e.PageIndex);
        }

        private async Task LoadSendMessagePage(int pageSize, int pageIndex)
        {
            SendMessage.StatusEnum? status = GetSelectedEnum<SendMessage.StatusEnum>(cboSendMessageStatus);
            SendMessage.TypeEnum? type = GetSelectedEnum<SendMessage.TypeEnum>(cboSendMessageType);
            string barcode = txtSendMessageBarcode.Text.Trim();
            string sampleNo = txtSendMessageSampleNo.Text.Trim();
            long createTimeStart = new DateTimeOffset(dtpSendMessageCreateTimeStart.Value.Date).ToUnixTimeMilliseconds();
            long createTimeEnd = new DateTimeOffset(dtpSendMessageCreateTimeEnd.Value.Date.AddDays(1).AddMilliseconds(-1)).ToUnixTimeMilliseconds();
            Page<SendMessagePageItem> page = await SendMessageService.Instance.GetPage(_instrumentId, _messageEncoding, status, type, barcode, sampleNo, createTimeStart, createTimeEnd, pageSize, pageIndex);
            dgvSendMessage.DataSource = page.Data;
            pagerSendMessage.SetPageInfo(page);
        }
    }
}

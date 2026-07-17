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
            //cboReceiveMessageStatus.SelectedValue = ((int)ReceiveMessage.StatusEnum.Pending).ToString();
            //cboReceiveMessageType.SelectedValue = ((int)ReceiveMessageDecode.TypeEnum.TestResult).ToString();
            dtpReceiveMessageCreateTimeStart.Value = DateTime.Today;
            dtpReceiveMessageCreateTimeEnd.Value = DateTime.Today;
            dgvReceiveMessage.ColumnHeaderMouseClick += dgvReceiveMessage_ColumnHeaderMouseClick;
        }

        private void dgvReceiveMessage_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != colReceiveMessageSelect.Index || dgvReceiveMessage.Rows.Count == 0)
            {
                return;
            }
            dgvReceiveMessage.EndEdit();
            if (colReceiveMessageSelect.HeaderText == "全选")
            {
                colReceiveMessageSelect.HeaderText = "反全选";
                foreach (DataGridViewRow row in dgvReceiveMessage.Rows)
                {
                    row.Cells[colReceiveMessageSelect.Index].Value = true;
                }
            }
            else
            {
                colReceiveMessageSelect.HeaderText = "全选";
                foreach (DataGridViewRow row in dgvReceiveMessage.Rows)
                {
                    row.Cells[colReceiveMessageSelect.Index].Value = false;
                }
            }
        }

        private async Task RefreshReceiveMessage()
        {
            await LoadReceiveMessagePage(pagerReceiveMessage.PageSize, pagerReceiveMessage.PageIndex);
        }

        private async void btnReceiveMessageQuery_Click(object sender, EventArgs e)
        {
            await LoadReceiveMessagePage(pagerReceiveMessage.PageSize, 1);
        }

        private async void PagerReceiveMessage_PageChanged(object? sender, PagerChangedEventArgs e)
        {
            await LoadReceiveMessagePage(e.PageSize, e.PageIndex);
        }

        private async Task LoadReceiveMessagePage(int pageSize, int pageIndex)
        {
            ReceiveMessage.StatusEnum? status = GetSelectedEnum<ReceiveMessage.StatusEnum>(cboReceiveMessageStatus);
            ReceiveMessageDecode.TypeEnum? type = GetSelectedEnum<ReceiveMessageDecode.TypeEnum>(cboReceiveMessageType);
            string barcode = txtReceiveMessageBarcode.Text.Trim();
            string sampleNo = txtReceiveMessageSampleNo.Text.Trim();
            long createTimeStart = new DateTimeOffset(dtpReceiveMessageCreateTimeStart.Value.Date).ToUnixTimeMilliseconds();
            long createTimeEnd = new DateTimeOffset(dtpReceiveMessageCreateTimeEnd.Value.Date.AddDays(1).AddMilliseconds(-1)).ToUnixTimeMilliseconds();
            Page<ReceiveMessagePageItem> page = await ReceiveMessageService.Instance.GetPage(_instrumentId, _messageEncoding, status, type, barcode, sampleNo, createTimeStart, createTimeEnd, pageSize, pageIndex);
            dgvReceiveMessage.DataSource = page.Data;
            pagerReceiveMessage.SetPageInfo(page);
        }
    }
}

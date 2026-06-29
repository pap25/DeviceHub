using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis.Dto;
using DeviceHub.Win.Base;
using DeviceHub.Win.DeviceHubControl;
using System.IO.Ports;

namespace DeviceHub.Win
{
    public partial class DeviceStatus
    {
        private async Task initLisConfig(GetInstrument instrument, DriverConfig config)
        {
            lblLisConfigAuthCode.Text = Helper.MaskAuthCode(instrument.AuthCode);
            lblLisConfigAuthStatus.Text = Helper.FormatAuthStatus(instrument.Status);
            lblLisConfigExpireTime.Text = Helper.FormatExpireTime(instrument.ExpireTime);

            ClearCommConfigGroups();
            if (config.TcpConfig != null)
            {
                ShowTcpConfig(config.TcpConfig);
            }
            else if (config.SerialPortConfig != null)
            {
                ShowSerialPortConfig(config.SerialPortConfig);
            }

            await LoadInstrumentItemMappingPage(pagerInstrumentItemMapping.PageIndex, pagerInstrumentItemMapping.PageSize);
        }

        private async Task RefreshLisConfig()
        {
            GetInstrument instrument = await lisClient.GetInstrument(_instrumentId);
            DriverConfig config = await lisClient.GetDriverConfig(_instrumentId);
            await initLisConfig(instrument, config);
        }

        private void ClearCommConfigGroups()
        {
            for (int i = pnlLisConfigLeft.Controls.Count - 1; i >= 0; i--)
            {
                Control control = pnlLisConfigLeft.Controls[i];
                if (control == grpLisConfigAuthInfo)
                {
                    continue;
                }

                pnlLisConfigLeft.Controls.Remove(control);
                control.Dispose();
            }
        }

        private async void PagerInstrumentItemMapping_PageChanged(object? sender, PagerChangedEventArgs e)
        {
            await LoadInstrumentItemMappingPage(e.PageSize, e.PageIndex);
        }

        private async Task LoadInstrumentItemMappingPage(int pageSize, int pageIndex)
        {
            Page<GetInstrumentItemMappingPage> page = await lisClient.GetInstrumentItemMappingPage(_instrumentId, pageSize, pageIndex);
            dgvInstrumentItemMapping.DataSource = page.Data;
            pagerInstrumentItemMapping.SetPageInfo(page);
        }

        private void ShowTcpConfig(TcpConfig tcpConfig)
        {
            CreateCommConfigGroup("网口参数",
            [
                ("IP地址", tcpConfig.Host ?? string.Empty, null),
                ("端口", tcpConfig.Port.ToString(), null),
                ("连接模式", "服务端（Server）", null),
                ("编码方式", tcpConfig.Encoding ?? string.Empty, null),
                ("状态", "监听中", Color.Green),
                ("已连客户端", "172.28.80.1:60941", null),
            ]);
        }

        private void ShowSerialPortConfig(SerialPortConfig serialPortConfig)
        {
            CreateCommConfigGroup("串口参数",
            [
                ("COM口", serialPortConfig.PortName ?? string.Empty, null),
                ("波特率", serialPortConfig.BaudRate.ToString(), null),
                ("校验位", ((Parity)serialPortConfig.Parity).ToString(), null),
                ("数据位", serialPortConfig.DataBits.ToString(), null),
                ("停止位", ((StopBits)serialPortConfig.StopBits).ToString(), null),
                ("编码方式", serialPortConfig.Encoding ?? string.Empty, null),
                ("串口状态", "已打开", Color.Green),
            ]);
        }

        private void CreateCommConfigGroup(string title, (string label, string value, Color? valueColor)[] fields)
        {
            const int labelX = 20;
            const int valueX = 100;
            const int startY = 43;
            const int rowHeight = 35;
            const int bottomMargin = 32;

            var grp = new GroupBox
            {
                Text = title,
                ForeColor = Color.DarkBlue,
                Location = new Point(3, grpLisConfigAuthInfo.Bottom + 6),
                Size = new Size(244, startY + (fields.Length - 1) * rowHeight + bottomMargin),
            };

            for (int i = 0; i < fields.Length; i++)
            {
                int y = startY + i * rowHeight;
                var (label, value, valueColor) = fields[i];

                var lblName = new Label
                {
                    AutoSize = true,
                    ForeColor = SystemColors.ControlText,
                    Location = new Point(labelX, y),
                    Text = label + "：",
                };

                var lblValue = new Label
                {
                    AutoSize = true,
                    ForeColor = valueColor ?? SystemColors.ControlText,
                    Location = new Point(valueX, y),
                    Text = value,
                };

                grp.Controls.Add(lblName);
                grp.Controls.Add(lblValue);
            }

            pnlLisConfigLeft.Controls.Add(grp);
        }
    }
}

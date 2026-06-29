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
            // 授权信息
            lblAuthCode.Text = Helper.MaskAuthCode(instrument.AuthCode);
            lblAuthStatus.Text = Helper.FormatAuthStatus(instrument.Status);
            lblExpireTime.Text = Helper.FormatExpireTime(instrument.ExpireTime);

            // 显示通信配置
            if (config.TcpConfig != null)
            {
                ShowTcpConfig(config.TcpConfig);
            }
            else if (config.SerialPortConfig != null)
            {
                ShowSerialPortConfig(config.SerialPortConfig);
            }

            // 仪器项目映射列表
            await LoadInstrumentItemMappingPageAsync(1, pagerInstrumentItemMapping.PageSize);
        }

        private async void PagerInstrumentItemMapping_PageChanged(object? sender, PagerChangedEventArgs e)
        {
            await LoadInstrumentItemMappingPageAsync(e.PageIndex, e.PageSize);
        }

        private async Task LoadInstrumentItemMappingPageAsync(int pageIndex, int pageSize)
        {
            Page<GetInstrumentItemMappingPage> page = await lisClient.GetInstrumentItemMappingPage(_instrumentId, pageIndex, pageSize);
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
                Location = new Point(3, grpAuthInfo.Bottom + 6),
                Size = new Size(250, startY + (fields.Length - 1) * rowHeight + bottomMargin),
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

            tabPage1.Controls.Add(grp);
        }
    }
}

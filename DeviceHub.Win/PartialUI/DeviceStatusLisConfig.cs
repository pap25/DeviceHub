using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis.Dto;
using DeviceHub.Win.Base;
using DeviceHub.Win.DeviceHubControl;
using System.IO.Ports;

namespace DeviceHub.Win
{
    public partial class DeviceStatus
    {
        private GroupBox? grpLisConfigAuthInfo;
        private Label? _lblSerialStatus;
        private Label? _lblTcpClientEndpoint;
        private System.Windows.Forms.Timer? _commStatusTimer;

        private void initDriverConfig(DriverConfig config)
        {
            ClearCommConfigGroups();
            if (config.TcpConfig != null)
            {
                ShowTcpConfig(config.TcpConfig, "监听中", Color.Green);
            }
            else if (config.SerialPortConfig != null)
            {
                ShowSerialPortConfig(config.SerialPortConfig, serialDeviceDriver == null ? "" : serialDeviceDriver.GetLineStateName(), Color.Green);
            }
        }

        private void initDriverConfigStatusEmpty(DriverConfig config)
        {
            ClearCommConfigGroups();
            if (config.TcpConfig != null)
            {
                ShowTcpConfig(config.TcpConfig, "", Color.Green);
            }
            else if (config.SerialPortConfig != null)
            {
                ShowSerialPortConfig(config.SerialPortConfig, "", Color.Green);
            }
        }

        private async Task initLisConfig(GetInstrument instrument, DriverConfig config, bool isError)
        {
            ShowAuthInfoGroup(
                Helper.MaskAuthCode(instrument.AuthCode),
                Helper.FormatAuthStatus(instrument.Status),
                Helper.FormatExpireTime(instrument.ExpireTime));

            if (!isError)
            {
                initDriverConfig(config);
            }
            else
            {
                initDriverConfigStatusEmpty(config);
            }

            await LoadInstrumentItemMappingPage(pagerInstrumentItemMapping.PageSize, pagerInstrumentItemMapping.PageIndex);
        }

        private async Task RefreshLisConfig(bool isError)
        {
            GetInstrument instrument = await lisClient.GetInstrument(_instrumentId);
            DriverConfig config = await lisClient.GetDriverConfig(_instrumentId);
            await initLisConfig(instrument, config, isError);
        }

        private void ClearCommConfigGroups()
        {
            _lblSerialStatus = null;
            _lblTcpClientEndpoint = null;

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

        private void StartCommStatusTimer()
        {
            _commStatusTimer ??= new System.Windows.Forms.Timer { Interval = 3000 };
            _commStatusTimer.Tick -= CommStatusTimer_Tick;
            _commStatusTimer.Tick += CommStatusTimer_Tick;
            _commStatusTimer.Start();
            UpdateCommStatusDisplay();
        }

        private void StopCommStatusTimer()
        {
            if (_commStatusTimer == null)
            {
                return;
            }

            _commStatusTimer.Stop();
            _commStatusTimer.Tick -= CommStatusTimer_Tick;
        }

        private void CommStatusTimer_Tick(object? sender, EventArgs e)
        {
            UpdateCommStatusDisplay();
        }

        private void UpdateCommStatusDisplay()
        {
            if (tabControl1.SelectedTab != tabLisConfig)
            {
                return;
            }

            if (_lblSerialStatus != null && serialDeviceDriver != null)
            {
                _lblSerialStatus.Text = serialDeviceDriver.GetLineStateName();
            }

            if (_lblTcpClientEndpoint != null && tcpDeviceDriver != null)
            {
                _lblTcpClientEndpoint.Text = tcpDeviceDriver.GetClientRemoteEndPoint();
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

        private void ShowAuthInfoGroup(string authCode, string authStatus, string expireTime)
        {
            if (grpLisConfigAuthInfo != null)
            {
                pnlLisConfigLeft.Controls.Remove(grpLisConfigAuthInfo);
                grpLisConfigAuthInfo.Dispose();
                grpLisConfigAuthInfo = null;
            }

            grpLisConfigAuthInfo = CreateConfigGroup("授权信息", new Point(3, 6),
            [
                ("授权码", authCode, Color.Red),
                ("状态", authStatus, Color.Green),
                ("过期时间", expireTime, null),
            ]);
            pnlLisConfigLeft.Controls.Add(grpLisConfigAuthInfo);
            pnlLisConfigLeft.Controls.SetChildIndex(grpLisConfigAuthInfo, 0);
        }

        private void ShowTcpConfig(TcpConfig tcpConfig, string status, Color statusColor)
        {
            CreateCommConfigGroup("网口参数",
            [
                ("IP地址", tcpConfig.Host ?? string.Empty, null),
                ("端口", tcpConfig.Port.ToString(), null),
                ("连接模式", "服务端（Server）", null),
                ("编码方式", tcpConfig.Encoding ?? string.Empty, null),
                ("状态", status, statusColor),
                ("已连客户端", tcpDeviceDriver != null ? tcpDeviceDriver.GetClientRemoteEndPoint() : string.Empty, null),
            ],
            trackClientEndpoint: true);
        }

        private void ShowSerialPortConfig(SerialPortConfig serialPortConfig, string status, Color statusColor)
        {
            CreateCommConfigGroup("串口参数",
            [
                ("COM口", serialPortConfig.PortName ?? string.Empty, null),
                ("波特率", serialPortConfig.BaudRate.ToString(), null),
                ("校验位", ((Parity)serialPortConfig.Parity).ToString(), null),
                ("数据位", serialPortConfig.DataBits.ToString(), null),
                ("停止位", ((StopBits)serialPortConfig.StopBits).ToString(), null),
                ("编码方式", serialPortConfig.Encoding ?? string.Empty, null),
                ("状态", status, statusColor),
            ],
            trackStatus: true);
        }

        private void CreateCommConfigGroup(string title, (string label, string value, Color? valueColor)[] fields, bool trackStatus = false, bool trackClientEndpoint = false)
        {
            int top = grpLisConfigAuthInfo != null ? grpLisConfigAuthInfo.Bottom + 6 : 6;
            var grp = CreateConfigGroup(title, new Point(3, top), fields, trackStatus, trackClientEndpoint);
            pnlLisConfigLeft.Controls.Add(grp);
        }

        private GroupBox CreateConfigGroup(string title, Point location, (string label, string value, Color? valueColor)[] fields, bool trackStatus = false, bool trackClientEndpoint = false)
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
                Location = location,
                Size = new Size(244, startY + (fields.Length - 1) * rowHeight + bottomMargin),
                TabStop = false,
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

                if (trackStatus && label == "状态")
                {
                    _lblSerialStatus = lblValue;
                }
                else if (trackClientEndpoint && label == "已连客户端")
                {
                    _lblTcpClientEndpoint = lblValue;
                }
            }

            return grp;
        }
    }
}

using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Base.Common;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Win.Base;
using DeviceHub.Win.DeviceHubControl;
using DeviceHub.Win.Utils;
using System.Drawing;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static DeviceHub.Lis.Dto.GetInstrument;

namespace DeviceHub.Win
{
    public partial class DeviceStatus : Form
    {
        private readonly ILisClient lisClient = LisClient.Instance;
        private int _instrumentId;

        public DeviceStatus()
        {
            InitializeComponent();
            pagerInstrumentItemMapping.PageChanged += PagerInstrumentItemMapping_PageChanged;
        }
        private async void DeviceStatus_Shown(object sender, EventArgs e)
        {
            string? driverIdStr = AppConfig.Configuration["DeviceId"];
            if (driverIdStr == null)
            {
                MessageBox.Show("没有配置设备ID");
                return;
            }
            int driverId = int.Parse(driverIdStr);

            GetInstrument instrument = await lisClient.GetInstrument(driverId);
            if (instrument == null)
            {
                MessageBox.Show($"LIS上没有这仪器driverId: {driverId}");
                return;
            }

            DriverConfig config = await lisClient.GetDriverConfig(driverId);
            await initLisConfig(instrument, config);

            Resp resp;
            if (config.TcpConfig != null)
            {
                ITcpDeviceDriver yhloTestDriver = DriverFactory.Create<ITcpDeviceDriver>();
                resp = await yhloTestDriver.Start(config.TcpConfig);
            }
            else if (config.SerialPortConfig != null)
            {
                ISerialDeviceDriver serialDeviceDriver = DriverFactory.Create<ISerialDeviceDriver>();
                resp = await serialDeviceDriver.Start(config.SerialPortConfig);
            }
            else
            {
                MessageBox.Show($"LIS上没有配置这仪器连接方式driverId: {driverId}");
                return;
            }
            if (!resp.IsSuccess())
            {
                MessageBox.Show($"启动失败errorMsg: {resp.GetErrorMsg()}");
                return;
            }
        }

        private async Task initLisConfig(GetInstrument instrument, DriverConfig config)
        {
            // 窗口title
            _instrumentId = instrument.InstrumentId;
            this.Text += $" {instrument.InstrumentModel} {instrument.InstrumentName} {instrument.InstrumentId}";

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

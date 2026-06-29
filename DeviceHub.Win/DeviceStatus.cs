using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Win.Base;

namespace DeviceHub.Win
{
    public partial class DeviceStatus : Form
    {
        private readonly ILisClient lisClient = LisClient.Instance;
        private int _instrumentId;
        private bool _isReady;

        public DeviceStatus()
        {
            InitializeComponent();
            pagerInstrumentItemMapping.PageChanged += PagerInstrumentItemMapping_PageChanged;
            pagerReceiveMessage.PageChanged += PagerReceiveMessage_PageChanged;
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
        }

        private async void TabControl1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!_isReady)
            {
                return;
            }

            await RefreshCurrentTabAsync();
        }

        private async Task RefreshCurrentTabAsync()
        {
            if (tabControl1.SelectedTab == tabPageLisConfig)
            {
                await RefreshLisConfigAsync();
            }
            else if (tabControl1.SelectedTab == tabPageReceiveMessage)
            {
                await RefreshReceiveMessageAsync();
            }
        }
        private async void DeviceStatus_Shown(object sender, EventArgs e)
        {
            string? instrumentIdStr = AppConfig.Configuration["InstrumentId"];
            if (instrumentIdStr == null)
            {
                MessageBox.Show("没有配置仪器ID");
                return;
            }
            _instrumentId = int.Parse(instrumentIdStr);

            GetInstrument instrument = await lisClient.GetInstrument(_instrumentId);
            if (instrument == null)
            {
                MessageBox.Show($"LIS上没有这仪器instrumentId: {_instrumentId}");
                return;
            }
            this.Text += $" {instrument.InstrumentModel} {instrument.InstrumentName} {instrument.InstrumentId}"; // 窗口title

            DriverConfig config = await lisClient.GetDriverConfig(_instrumentId);
            await ApplyLisConfigAsync(instrument, config);
            await initReceiveMessage();
            _isReady = true;

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
                MessageBox.Show($"LIS上没有配置这仪器连接方式instrumentId: {_instrumentId}");
                return;
            }
            if (!resp.IsSuccess())
            {
                MessageBox.Show($"启动失败errorMsg: {resp.GetErrorMsg()}");
                return;
            }
        }
    }
}

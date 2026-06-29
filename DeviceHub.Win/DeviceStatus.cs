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
            _instrumentId = instrument.InstrumentId;
            this.Text += $" {instrument.InstrumentModel} {instrument.InstrumentName} {instrument.InstrumentId}"; // 窗口title

            DriverConfig config = await lisClient.GetDriverConfig(driverId);
            await initLisConfig(instrument, config);
            await initReceiveMessage();

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
    }
}

using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Utils;
using DeviceHub.Win.Base;

namespace DeviceHub.Win
{
    public partial class DeviceStatus : Form
    {
        private readonly ILisClient lisClient = LisClient.Instance;
        private int _instrumentId;
        private bool _isReady;
        private bool _isError = false;
        private ITcpDeviceDriver? tcpDeviceDriver = null;
        private ISerialDeviceDriver? serialDeviceDriver = null;

        public DeviceStatus()
        {
            InitializeComponent();
            pagerInstrumentItemMapping.PageChanged += PagerInstrumentItemMapping_PageChanged;
            pagerReceiveMessage.PageChanged += PagerReceiveMessage_PageChanged;
            pagerSendMessage.PageChanged += PagerSendMessage_PageChanged;
            pagerLog.PageChanged += PagerLog_PageChanged;
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
            await initLisConfig(instrument, config, _isError);
            await initReceiveMessage();
            await initSendMessage();
            await initLog();
            _isReady = true;

            try
            {
                if (config.TcpConfig != null)
                {
                    tcpDeviceDriver = DriverFactory.Create<ITcpDeviceDriver>();
                    await tcpDeviceDriver.Start(_instrumentId, config.TcpConfig);
                }
                else if (config.SerialPortConfig != null)
                {
                    serialDeviceDriver = DriverFactory.Create<ISerialDeviceDriver>();
                    await serialDeviceDriver.Start(_instrumentId, config.SerialPortConfig);
                }
                else
                {
                    MessageBox.Show($"LIS上没有配置这仪器连接方式instrumentId: {_instrumentId}");
                    return;
                }
            }
            catch (Exception ex)
            {
                _isError = true;
                initDriverConfigStatusEmpty(config);
                Logger.Error(nameof(DeviceStatus), "启动失败！请检查DLL、网络环境、端口是否被占用然后重启", ex);
                MessageBox.Show($"启动失败！请检查DLL、网络环境、端口是否被占用然后重启: {ex.Message}");
                return;
            }
        }

        private async void TabControl1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!_isReady)
            {
                return;
            }

            await RefreshCurrentTab();
        }

        private async Task RefreshCurrentTab()
        {
            if (tabControl1.SelectedTab == tabLisConfig)
            {
                await RefreshLisConfig(_isError);
            }
            else if (tabControl1.SelectedTab == tabReceiveMessage)
            {
                await RefreshReceiveMessage();
            }
            else if (tabControl1.SelectedTab == tabSendMessage)
            {
                await RefreshSendMessage();
            }
            else if (tabControl1.SelectedTab == tabLog)
            {
                await RefreshLog();
            }
        }

        private static void BindEnumComboBox<TEnum>(ComboBox combo, bool isAll) where TEnum : Enum
        {
            var items = EnumExtensions.GetKeyValues<TEnum>();
            if (isAll)
                items.Insert(0, new EnumExtensions.KeyValue { Key = string.Empty, Value = "全部" });
            combo.DataSource = items;
            combo.DisplayMember = nameof(EnumExtensions.KeyValue.Value);
            combo.ValueMember = nameof(EnumExtensions.KeyValue.Key);
        }

        private TEnum? GetSelectedEnum<TEnum>(ComboBox comboBox) where TEnum : struct, Enum
        {
            if (comboBox.SelectedValue is not string key || string.IsNullOrWhiteSpace(key))
                return null;

            return (TEnum)Enum.ToObject(typeof(TEnum), byte.Parse(key));
        }
    }
}

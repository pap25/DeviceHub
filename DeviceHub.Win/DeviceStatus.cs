using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Base.Common;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Win.Base;
using DeviceHub.Win.DeviceHubControl;
using DeviceHub.Win.Utils;
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
            _instrumentId = instrument.InstrumentId;
            this.Text += $" {instrument.InstrumentModel} {instrument.InstrumentName} {instrument.InstrumentId}";

            lblAuthCode.Text = Helper.MaskAuthCode(instrument.AuthCode);
            lblAuthStatus.Text = Helper.FormatAuthStatus(instrument.Status);
            lblExpireTime.Text = Helper.FormatExpireTime(instrument.ExpireTime);

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

        //private static string FormatString(DriverConfig config)
        //{
        //    var sb = new StringBuilder();
        //    AppendVoFields(sb, config.SerialPortConfig);
        //    AppendVoFields(sb, config.TcpConfig);
        //    return sb.ToString();
        //}

        //private static void AppendVoFields(StringBuilder sb, object? vo)
        //{
        //    if (vo == null)
        //        return;

        //    foreach (PropertyInfo prop in vo.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //    {
        //        sb.AppendLine($"{prop.Name}: {prop.GetValue(vo)}");
        //    }
        //}

        //private async Task<Resp<DriverConfig>> LoadDriverConfig(int driverId)
        //{
        //    Logger.Info(nameof(DeviceStatus), $"=====================start driverId:{driverId}======================");

        //    try
        //    {
        //        DriverConfig config = await lisClient.GetDriverConfig(driverId);

        //        Logger.Info(nameof(DeviceStatus), $"从LIS拉取配置成功: {JsonSerializer.Serialize(config)}");

        //        return Resp<DriverConfig>.Ok(config);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(nameof(DeviceStatus), $"从LIS拉取配置失败 driverId:{driverId}", ex);
        //        return Resp<DriverConfig>.Fail($"从LIS拉取配置失败 driverId:{driverId}");
        //    }
        //}
    }
}

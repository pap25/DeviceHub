using DeviceHub.Abstractions;
using DeviceHub.Base.Common;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using DeviceHub.Win.Utils;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace DeviceHub.Win
{
    public partial class DeviceStatus : Form
    {
        private readonly ILisClient lisClient = LisClient.Instance;
        public DeviceStatus()
        {
            InitializeComponent();
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
            Resp<DriverConfig> respConfig = await LoadDriverConfig(driverId);
            if (!respConfig.IsSuccess())
            {
                lblConfig.Text = respConfig.GetErrorMsg();
                return;
            }
            DriverConfig? config = respConfig.GetData();
            lblConfig.Text = FormatString(config);

            try
            {
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
                    lblErrorMsg.Text = $"配置错误driverId:{driverId}";
                    Logger.Info(nameof(DeviceStatus), $"配置错误driverId:{driverId}");
                    return;
                }
                if (!resp.IsSuccess())
                {
                    lblErrorMsg.Text = resp.GetErrorMsg();
                }
            }
            catch (Exception ex)
            {
                lblErrorMsg.Text = ex.Message;
            }
        }

        private static string FormatString(DriverConfig config)
        {
            var sb = new StringBuilder();
            AppendVoFields(sb, config.SerialPortConfig);
            AppendVoFields(sb, config.TcpConfig);
            return sb.ToString();
        }

        private static void AppendVoFields(StringBuilder sb, object? vo)
        {
            if (vo == null)
                return;

            foreach (PropertyInfo prop in vo.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                sb.AppendLine($"{prop.Name}: {prop.GetValue(vo)}");
            }
        }

        private async Task<Resp<DriverConfig>> LoadDriverConfig(int driverId)
        {
            Logger.Info(nameof(DeviceStatus), $"=====================start driverId:{driverId}======================");

            try
            {
                DriverConfig config = await lisClient.queryDriverConfig(driverId);

                Logger.Info(nameof(DeviceStatus), $"从LIS拉取配置成功: {JsonSerializer.Serialize(config)}");

                return Resp<DriverConfig>.Ok(config);
            }
            catch (Exception ex)
            {
                Logger.Error(nameof(DeviceStatus), $"从LIS拉取配置失败 driverId:{driverId}", ex);
                return Resp<DriverConfig>.Fail($"从LIS拉取配置失败 driverId:{driverId}");
            }
        }
    }
}

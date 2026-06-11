using DeviceHub.Abstractions;
using DeviceHub.Base.Common;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
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
            Resp<DriverConfig> respConfig = await LoadDriverConfig(1);
            if (!respConfig.IsSuccess())
            {
                lblConfig.Text = respConfig.GetErrorMsg();
                return;
            }
            DriverConfig? config = respConfig.GetData();
            lblConfig.Text = FormatString(config);

            try
            {
                IDeviceDriver yhloTestDriver = DriverFactory.create();
                Resp resp = await yhloTestDriver.Start(config);
                if (!resp.IsSuccess())
                {
                    // 由于是异步线程，需Invoke在UI线程操作控件
                    lblErrorMsg.Invoke(new Action(() =>
                    {
                        lblErrorMsg.Text = resp.GetErrorMsg();
                    }));
                }
            }
            catch (Exception ex)
            {
                lblErrorMsg.Invoke(new Action(() =>
                {
                    lblErrorMsg.Text = ex.Message;
                }));
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
            Logger.Info($"=====================start driverId:{driverId}======================");

            try
            {
                DriverConfig config = await lisClient.queryDriverConfig(driverId);

                Logger.Info($"从LIS拉取配置成功: {JsonSerializer.Serialize(config)}");

                return Resp<DriverConfig>.Ok(config);
            }
            catch (Exception ex)
            {
                Logger.Error($"从LIS拉取配置失败 driverId:{driverId}", ex);
                return Resp<DriverConfig>.Fail($"从LIS拉取配置失败 driverId:{driverId}");
            }
        }
    }
}

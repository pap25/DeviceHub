using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Vo;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DeviceHub.Win
{
    public partial class DeviceStatus : Form
    {
        public DeviceStatus()
        {
            InitializeComponent();
        }

        private static string FormatResp(Resp resp)
        {
            if (!resp.IsSuccess())
                return resp.GetErrorMsg();

            var sb = new StringBuilder();
            AppendVoFields(sb, resp.GetSerialPortVo());
            AppendVoFields(sb, resp.GetNetworkPortVo());
            return sb.Length > 0 ? sb.ToString().TrimEnd() : "启动成功";
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

        private async void DeviceStatus_Shown(object sender, EventArgs e)
        {
            try
            {
                IDeviceDriver d = DriverFactory.create();
                Resp resp = await d.Start(1);
                lblMessage.Text = FormatResp(resp);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
    }
}

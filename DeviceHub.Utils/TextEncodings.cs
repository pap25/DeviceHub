using System.Text;

namespace DeviceHub.Utils
{
    public static class TextEncodings
    {
        private static Encoding? _gbk;

        /// <summary>
        /// 国内串口设备常用 GBK 编码。
        /// </summary>
        public static Encoding Gbk => _gbk ??= CreateGbk();

        private static Encoding CreateGbk()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GBK");
        }
    }
}

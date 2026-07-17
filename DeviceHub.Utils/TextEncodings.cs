using System.Text;

namespace DeviceHub.Utils
{
    public static class TextEncodings
    {
        private const string DefaultEncodingName = "UTF-8";

        static TextEncodings()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 根据配置的编码名称获取编码；未配置时使用 UTF-8。
        /// </summary>
        public static Encoding GetEncoding(string? encodingName)
        {
            return Encoding.GetEncoding(
                string.IsNullOrWhiteSpace(encodingName)
                    ? DefaultEncodingName
                    : encodingName.Trim());
        }

        /// <summary>
        /// 国内串口设备常用 GBK 编码。
        /// </summary>
        //public static Encoding Gbk => GetEncoding("GBK");
    }
}

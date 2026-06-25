namespace DeviceHub.Win
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            try
            {
                await Base.DatabaseInitializer.InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"数据库初始化失败：{ex.Message}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.Run(new DeviceStatus());
            //Application.Run(new Form1());
        }
    }
}
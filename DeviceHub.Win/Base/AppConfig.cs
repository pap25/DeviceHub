using Microsoft.Extensions.Configuration;

namespace DeviceHub.Win.Base
{
    internal static class AppConfig
    {
        public static IConfigurationRoot Configuration { get; }

        /// <summary>
        /// SQLite 连接字符串
        /// </summary>
        public static string DatabaseConnectionString =>
            Configuration["Database:ConnectionString"]
            ?? throw new InvalidOperationException("Database:ConnectionString 未配置。");

        static AppConfig()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config/appsettings.json", optional: false)
                .Build();
        }
    }
}

using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace DeviceHub.Win
{
    internal class DriverFactory
    {
        public static T create<T>()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var dll = config["Driver:Assembly"]
                ?? throw new InvalidOperationException("Driver:Assembly is not configured.");
            var typeName = config["Driver:Type"]
                ?? throw new InvalidOperationException("Driver:Type is not configured.");

            Assembly asm = Assembly.LoadFrom(dll);

            var type = asm.GetType(typeName)
                ?? throw new InvalidOperationException($"Type '{typeName}' not found in assembly '{dll}'.");

            return (T)Activator.CreateInstance(type)!;
        }
    }
}

using DeviceHub.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace DeviceHub.Win
{
    internal class DriverFactory
    {
        public static IDeviceDriver create()
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

            return (IDeviceDriver)Activator.CreateInstance(type)!;
        }
    }
}

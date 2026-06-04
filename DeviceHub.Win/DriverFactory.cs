using DeviceHub.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DeviceHub.Win
{
    internal class DriverFactory
    {
        public static IDeviceDriver create()
        {
            String dll = "DeviceHub.Yhlo.dll";

            Assembly asm = Assembly.LoadFrom(dll);

            var type = asm.GetType("DeviceHub.Yhlo.TestDriver");

            return (IDeviceDriver)Activator.CreateInstance(type);
        }

    }
}

using System.Reflection;

namespace DeviceHub.Win.Utils
{
    internal class DriverFactory
    {
        public static T Create<T>() where T : class
        {
            var dll = AppConfig.Configuration["Driver:Assembly"]
                ?? throw new InvalidOperationException("Driver:Assembly is not configured.");

            Assembly asm = Assembly.LoadFrom(dll);

            var type = asm.GetTypes()
                .FirstOrDefault(t =>
                    typeof(T).IsAssignableFrom(t) &&
                    t.IsClass &&
                    !t.IsAbstract);

            if (type == null)
            {
                throw new InvalidOperationException(
                    $"DLL {dll} does not contain implementation of {typeof(T).Name}");
            }

            return Activator.CreateInstance(type) as T
                ?? throw new InvalidOperationException(
                    $"Failed to create {type.FullName}");
        }
    }
}

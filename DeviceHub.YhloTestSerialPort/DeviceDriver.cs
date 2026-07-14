using DeviceHub.Abstractions;
using DeviceHub.Abstractions.Dto;
using DeviceHub.Utils;
using DeviceHub.Model.Entities;
using DeviceHub.YhloTestSerialPort.Handler;
using DeviceHub.Template.Template.SerialPort;

namespace DeviceHub.YhloTestSerialPort
{
    public class DeviceDriver : ISerialDeviceDriver
    {
        private readonly string logType = nameof(DeviceDriver);
        private IConsumeTask receiveTask = null!;
        private SerialPortSession? session;

        public async Task Start(long instrumentId, SerialPortConfig config)
        {
            receiveTask = new BatchConsumeTask<ReceiveMessage>(new ReceiveHandler(instrumentId));
            receiveTask.StartConsume();

            session = new SerialPortSession(instrumentId, receiveTask, new SendHandler(instrumentId));
            await session.Start(config);

            Logger.Info(logType, $"设备驱动已启动 instrumentId={instrumentId}, port={config.PortName}");
        }

        public void Stop()
        {
            receiveTask?.Shutdown();
            session?.Dispose();
            session = null;
            Logger.Info(logType, $"设备驱动已停止");
        }
    }
}
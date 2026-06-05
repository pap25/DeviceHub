using DeviceHub.Lis.Dto;

namespace DeviceHub.Lis
{
    public class LisClient
    {
        public SerialPortConfig querySerialPortConfigById(int id)
        {
            //if (id == 1)
            //{
            //    throw new Exception("调用LIS异常");
            //}
            return new SerialPortConfig
            {
                PortName = "COM4",
                BaudRate = 9600,
                Parity = 0,
                DataBits = 8,
                StopBits = 1
            };
        }
    }
}

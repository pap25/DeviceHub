
namespace DeviceHub.Abstractions.Vo
{
    public class Resp
    {
        private int code = 200;
        private string? errorMsg;
        private SerialPortVo serialPortVo;
        private NetworkPortVo networkPortVo;

        public int GetCode()
        {
            return code;
        }

        public string? GetErrorMsg()
        {
            return errorMsg;
        }

        public SerialPortVo GetSerialPortVo()
        {
            return serialPortVo;
        }

        public NetworkPortVo GetNetworkPortVo()
        {
            return networkPortVo;
        }

        private Resp()
        {

        }
        private Resp(int code, string errorMsg)
        {
            this.code = code;
            this.errorMsg = errorMsg;
        }

        public bool IsSuccess()
        {
            return code == 200;
        }

        public static Resp Make(string errorMsg)
        {
            Resp resp = new(500, errorMsg);
            return resp;
        }

        public static Resp Make(int code, string errorMsg)
        {
            Resp resp = new(code, errorMsg);
            return resp;
        }

        public static Resp Ok(SerialPortVo serialPortVo)
        {
            Resp resp = new();
            resp.serialPortVo = serialPortVo;
            return resp;
        }

        public static Resp Ok(NetworkPortVo networkPortVo)
        {
            Resp resp = new();
            resp.networkPortVo = networkPortVo;
            return resp;
        }
    }
}

namespace DeviceHub.Abstractions
{
    public class Resp
    {
        protected int code = 200;
        protected string? errorMsg;

        protected Resp() { }

        protected Resp(int code, string? errorMsg)
        {
            this.code = code;
            this.errorMsg = errorMsg;
        }

        public int GetCode()
        {
            return code;
        }

        public string? GetErrorMsg()
        {
            return errorMsg;
        }

        public bool IsSuccess()
        {
            return code == 200;
        }
        public static Resp Fail(string errorMsg)
        {
            return new(500, errorMsg);
        }

        public static Resp Fail(int code, string errorMsg)
        {
            return new(code, errorMsg);
        }

        public static Resp Ok()
        {
            return new();
        }
    }

    public class Resp<T> : Resp
    {
        private T? data;

        private Resp()
        {
        }

        public T? GetData()
        {
            return data;
        }

        public static Resp<T> Fail(string errorMsg)
        {
            Resp<T> resp = new();
            resp.code = 500;
            resp.errorMsg = errorMsg;
            return resp;
        }

        public static Resp<T> Fail(int code, string errorMsg)
        {
            Resp<T> resp = new();
            resp.code = code;
            resp.errorMsg = errorMsg;
            return resp;
        }

        public static Resp<T> Ok(T data)
        {
            Resp<T> resp = new();
            resp.data = data;
            return resp;
        }
    }
}
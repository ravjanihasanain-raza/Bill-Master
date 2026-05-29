namespace Bill_Master.Model
{
    public class ResponseResult
    {
        public ResponseResult()
        {

        }

        // 2 PARAMETER
        public ResponseResult(string status, object result)
        {
            Status = status;
            Result = result;
        }

        // 3 PARAMETER
        public ResponseResult(string status, string message, object result)
        {
            Status = status;
            Message = message;
            Result = result;
        }

        // STATUS + MESSAGE
        public ResponseResult(string status, string message)
        {
            Status = status;
            Message = message;
        }

        public string Status { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public object? Result { get; set; }
    }
}
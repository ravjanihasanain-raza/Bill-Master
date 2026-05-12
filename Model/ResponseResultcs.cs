namespace Bill_Master.Model
{
    public class ResponseResult
    {
        public ResponseResult(string status, object result)
        {
            Status = status;
            Result = result;
        }

        public string Status { get; set; }
        public object Result { get; set; }
    }
}

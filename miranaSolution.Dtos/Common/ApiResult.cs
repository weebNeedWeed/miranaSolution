namespace miranaSolution.Dtos.Common
{
    public class ApiResult<TData>
    {
        public bool IsSucceed { get; set; }
        public TData Data { get; set; }
        public string Message { get; set; }
    }
}
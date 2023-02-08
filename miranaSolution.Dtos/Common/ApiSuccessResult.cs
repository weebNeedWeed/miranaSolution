namespace miranaSolution.Dtos.Common
{
    public class ApiSuccessResult<TData> : ApiResult<TData>
    {
        public ApiSuccessResult()
        {
            IsSucceed = true;
        }

        public ApiSuccessResult(TData dataObj)
        {
            IsSucceed = true;
            Data = dataObj;
        }
    }
}
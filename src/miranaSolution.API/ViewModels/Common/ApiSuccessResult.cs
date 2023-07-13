namespace miranaSolution.API.ViewModels.Common;

public class ApiSuccessResult<TData> : ApiResult<TData>
{
    public ApiSuccessResult()
    {
        Status = "success";
    }

    public ApiSuccessResult(TData dataObj)
    {
        Status = "success";
        Data = dataObj;
    }
}
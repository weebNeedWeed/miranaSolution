namespace miranaSolution.DTOs.Common;

public class ApiErrorResult : ApiResult<string>
{
    public ApiErrorResult()
    {
        Status = "error";
    }

    public ApiErrorResult(string errorMsg)
    {
        Status = "error";
        Message = errorMsg;
    }
}
using miranaSolution.DTOs.Common;

namespace miranaSolution.API.ViewModels.Common;

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
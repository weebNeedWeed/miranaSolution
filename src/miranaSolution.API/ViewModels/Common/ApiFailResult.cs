namespace miranaSolution.API.ViewModels.Common;

public class ApiFailResult : ApiResult<Dictionary<string, string>>
{
    public ApiFailResult()
    {
        Status = "fail";
    }

    public ApiFailResult(Dictionary<string, string> validationMessage)
    {
        Status = "fail";
        Data = validationMessage;
    }
}
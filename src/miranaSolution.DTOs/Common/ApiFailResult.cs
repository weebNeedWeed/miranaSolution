namespace miranaSolution.DTOs.Common;

public class ApiFailResult : ApiResult<Dictionary<string, List<string>>>
{
    public ApiFailResult()
    {
        Status = "fail";
    }

    public ApiFailResult(Dictionary<string, List<string>> validationMessage)
    {
        Status = "fail";
        Data = validationMessage;
    }
}
namespace miranaSolution.API.ViewModels.Common;

public class ApiResult<TData>
{
    public string Status { get; set; }
    public TData Data { get; set; }
    public string Message { get; set; }
}
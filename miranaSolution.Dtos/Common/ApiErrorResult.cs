namespace miranaSolution.Dtos.Common
{
  public class ApiErrorResult<TData> : ApiResult<TData>
  {
    public ApiErrorResult()
    {
      IsSucceed = false;
    }

    public ApiErrorResult(string msg)
    {
      IsSucceed = false;
      Message = msg;
    }
  }
}
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using miranaSolution.API.ViewModels.Common;

namespace miranaSolution.API.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    public ApiExceptionFilter(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public void OnException(ExceptionContext context)
    {
        if (!_webHostEnvironment.IsDevelopment())
        {
            // TODO: Implementing logging here
            context.Result = new JsonResult(
                new ApiErrorResult("Có lỗi xảy ra, vui lòng thử lại!"));
        }
    }
}
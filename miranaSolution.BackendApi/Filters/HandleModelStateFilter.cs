using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using miranaSolution.Dtos.Common;

namespace miranaSolution.BackendApi.Filters
{
    public class HandleModelStateFilter : IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var errors = new Dictionary<string, List<string>>();

            if (!context.ModelState.IsValid)
            {
                foreach (var key in context.ModelState.Keys)
                {
                    var errorMessages = context.ModelState[key].Errors
                        .Select(x => x.ErrorMessage).ToList();
                    if (errorMessages.Count > 0)
                        errors.Add(key, errorMessages);
                }

                context.Result = new JsonResult(new ApiFailResult(errors));
            }

            return Task.FromResult(0);
        }
    }
}
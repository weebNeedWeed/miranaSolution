﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace miranaSolution.API.Filters;

public class ModelStateFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // if (!context.ModelState.IsValid)
        // {
        //     var errors = new Dictionary<string, List<string>>();
        //
        //     foreach (var key in context.ModelState.Keys)
        //     {
        //         var errorMessages = context.ModelState[key].Errors
        //             .Select(x => x.ErrorMessage).ToList();
        //         if (errorMessages.Count > 0)
        //             errors.Add(key, errorMessages);
        //     }
        //
        //     context.Result = new JsonResult(new ApiFailResult(errors));
        //
        //     return;
        // }
        //
        // await next();
    }
}
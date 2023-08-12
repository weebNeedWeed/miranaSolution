using Microsoft.AspNetCore.Mvc;
using miranaSolution.Admin.ViewModels.Common;
using miranaSolution.DTOs.Common;

namespace miranaSolution.Admin.Controllers.Components;

public class PagerViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PagerResult pagerResult)
    {
        return await Task.FromResult(View("Default", pagerResult));
    }
}
using Microsoft.AspNetCore.Mvc;
using miranaSolution.DTOs.Common;

namespace miranaSolution.Admin.Controllers.Components;

public class PagerViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PagedResultBase pagedResult)
    {
        return await Task.FromResult(View("Default", pagedResult));
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Slides;
using miranaSolution.Services.Catalog.Slides;
using miranaSolution.DTOs.Catalog.Slides;
using miranaSolution.DTOs.Common;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RolesConstant.Administrator)]
public class SlidesController : ControllerBase
{
    private readonly ISlideService _slideService;

    public SlidesController(ISlideService slideService)
    {
        _slideService = slideService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllSlides()
    {
        var getAllSlidesResponse = await _slideService.GetAllSlidesAsync();
        return Ok(new ApiSuccessResult<List<SlideVm>>(getAllSlidesResponse.SlideVms));
    }

    [HttpGet("{slideId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSlideById([FromRoute] int slideId)
    {
        var getSlideByIdResponse = await _slideService.GetSlideByIdAsync(
            new GetSlideByIdRequest(slideId));
        
        if (getSlideByIdResponse.SlideVm is null)
            return Ok(new ApiErrorResult("The slide with given Id does not exist."));

        return Ok(new ApiSuccessResult<SlideVm>(getSlideByIdResponse.SlideVm));
    }

    [HttpPost]
    public async Task<IActionResult> CreateSlide([FromBody] ApiCreateSlideRequest request)
    {
        var createSlideResponse = await _slideService.CreateSlideAsync(
            new CreateSlideRequest(
                request.Name,
                request.ShortDescription,
                request.ThumbnailImage,
                request.Genres,
                request.SortOrder));
        
        return Ok(new ApiSuccessResult<SlideVm>(createSlideResponse.SlideVm));
    }
}
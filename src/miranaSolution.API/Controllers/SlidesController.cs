using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Slides;
using miranaSolution.DTOs.Core.Slides;
using miranaSolution.Services.Core.Slides;
using miranaSolution.Services.Exceptions;
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
        return Ok(
            new ApiSuccessResult<ApiGetAllSlidesResponse>(
                new(getAllSlidesResponse.SlideVms)));
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
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateSlide([FromForm] ApiCreateSlideRequest request)
    {
        var stream = request.ThumbnailImage.OpenReadStream();
        var ext = Path.GetExtension(request.ThumbnailImage.FileName);

        try
        {
            var createSlideResponse = await _slideService.CreateSlideAsync(
                new CreateSlideRequest(
                    request.Name,
                    request.ShortDescription,
                    request.Genres,
                    request.SortOrder,
                    stream,
                    ext));

            return Ok(new ApiSuccessResult<SlideVm>(createSlideResponse.SlideVm));
        }
        catch (InvalidImageExtensionException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        } 
       
    }

    [HttpDelete("{slideId:int}")]
    public async Task<IActionResult> DeleteSlide([FromRoute] int slideId)
    {
        try
        {
            await _slideService.DeleteSlideAsync(
                new DeleteSlideRequest(slideId));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (SlideNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
    
    [HttpPut("{slideId:int}")]
    public async Task<IActionResult> UpdateSlide([FromRoute] int slideId,
        [FromBody] ApiUpdateSlideRequest request)
    {
        Stream? stream = null;
        string? ext = null;

        if (request.ThumbnailImage is not null)
        {
            stream = request.ThumbnailImage.OpenReadStream();
            ext = Path.GetExtension(request.ThumbnailImage.FileName);
        }
        
        try {
            var updateSlideResponse = await _slideService.UpdateSlideAsync(
                    new UpdateSlideRequest(
                        slideId,
                        request.Name,
                        request.ShortDescription,
                        request.Genres,
                        request.SortOrder,
                        stream,
                        ext));

            return Ok(new ApiSuccessResult<SlideVm>(updateSlideResponse.SlideVm));
        }
        catch (SlideNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (InvalidImageExtensionException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        } 
    }
}
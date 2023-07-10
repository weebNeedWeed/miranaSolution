using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetAll()
    {
        var slides = await _slideService.GetAll();
        return Ok(new ApiSuccessResult<List<SlideDto>>(slides));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var slide = await _slideService.GetById(id);
        if (slide is null)
            return Ok(new ApiFailResult(new Dictionary<string, List<string>>
            {
                { nameof(slide.Id), new List<string> { $"Invalid Id." } }
            }));

        return Ok(new ApiSuccessResult<SlideDto>(slide));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SlideCreateRequest request)
    {
        var newSlide = await _slideService.Create(request);
        return Ok(new ApiSuccessResult<SlideDto>(newSlide));
    }
}
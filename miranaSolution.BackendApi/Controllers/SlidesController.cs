using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Systems.Slides;
using miranaSolution.Data.Entities;
using miranaSolution.Dtos.Common;
using miranaSolution.Dtos.Systems.Slides;

namespace miranaSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidesController : ControllerBase
    {
        private readonly ISlideService _slideService;

        public SlidesController(ISlideService slideService)
        {
            _slideService = slideService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var slides = await _slideService.GetAll();
            return Ok(new ApiSuccessResult<List<SlideDto>>(slides));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var slide = await _slideService.GetById(id);
            if (slide is null)
            {
                return Ok(new ApiFailResult(new Dictionary<string, List<string>> {
                        { nameof(slide.Id), new List<string> { $"Invalid Id." } }
                    }));
            }

            return Ok(new ApiSuccessResult<SlideDto>(slide));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SlideCreateRequest request)
        {
            var newSlide = await _slideService.Create(request);
            return Ok(new ApiSuccessResult<SlideDto>(newSlide));
        }
    }
}
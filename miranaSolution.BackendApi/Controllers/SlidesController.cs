using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Common;
using miranaSolution.Data.Entities;
using miranaSolution.Dtos.Common;
using miranaSolution.Dtos.Systems.Slides;

namespace miranaSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidesController : ControllerBase
    {
        private readonly IGenericRepository<Slide> _slideRepository;

        public SlidesController(IGenericRepository<Slide> slideRepository)
        {
            _slideRepository = slideRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var slides = await _slideRepository.GetAllAsync();

            if (slides is null)
            {
                return BadRequest(new ApiErrorResult());
            }

            var returnedData = slides.Select(x => new SlideDto
            {
                Id = x.Id,
                Name = x.Name,
                ShortDescription = x.ShortDescription,
                Genres = x.Genres,
                SortOrder = x.SortOrder,
                ThumbnailImage = x.ThumbnailImage
            }).ToList();

            return Ok(new ApiSuccessResult<List<SlideDto>>(returnedData));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var slide = await _slideRepository.GetByIdAsync(id);

            if (slide is null)
            {
                return BadRequest(new ApiErrorResult($"Slide with id {id} does not exist."));
            }

            return Ok(new ApiSuccessResult<Slide>(slide));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SlideCreateRequest request)
        {
            var newSlide = new Slide
            {
                Name = request.Name,
                ShortDescription = request.ShortDescription,
                ThumbnailImage = request.ThumbnailImage,
                Genres = request.Genres,
                SortOrder = request.SortOrder
            };

            bool isSuccessful = await _slideRepository.InsertAsync(newSlide);

            if (!isSuccessful)
            {
                return BadRequest(new ApiErrorResult("Error when creating new Slide."));
            }

            return CreatedAtAction(nameof(GetById), new { id = newSlide.Id }, newSlide);
        }
    }
}
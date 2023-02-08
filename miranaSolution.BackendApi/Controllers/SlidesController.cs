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
                return BadRequest(new ApiErrorResult<bool>());
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
    }
}
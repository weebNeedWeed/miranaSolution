using miranaSolution.DTOs.Catalog.Slides;

namespace miranaSolution.Services.Catalog.Slides;

public interface ISlideService
{
    Task<List<SlideDto>> GetAll();

    Task<SlideDto> GetById(int id);

    Task<SlideDto> Create(SlideCreateRequest request);
}
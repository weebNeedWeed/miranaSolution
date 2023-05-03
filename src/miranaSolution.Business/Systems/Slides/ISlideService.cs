using miranaSolution.Dtos.Systems.Slides;

namespace miranaSolution.Business.Systems.Slides
{
    public interface ISlideService
    {
        Task<List<SlideDto>> GetAll();

        Task<SlideDto> GetById(int id);

        Task<SlideDto> Create(SlideCreateRequest request);
    }
}
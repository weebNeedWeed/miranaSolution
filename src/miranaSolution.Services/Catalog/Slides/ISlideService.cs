using miranaSolution.DTOs.Catalog.Slides;

namespace miranaSolution.Services.Catalog.Slides;

public interface ISlideService
{
    Task<GetAllSlidesResponse> GetAllSlidesAsync();

    Task<GetSlideByIdResponse> GetSlideByIdAsync(GetSlideByIdRequest request);

    Task<CreateSlideResponse> CreateSlideAsync(CreateSlideRequest request);
}
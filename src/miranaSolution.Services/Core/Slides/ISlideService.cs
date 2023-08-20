using miranaSolution.DTOs.Core.Slides;

namespace miranaSolution.Services.Core.Slides;

public interface ISlideService
{
    Task<GetAllSlidesResponse> GetAllSlidesAsync();

    Task<GetSlideByIdResponse> GetSlideByIdAsync(GetSlideByIdRequest request);

    Task<CreateSlideResponse> CreateSlideAsync(CreateSlideRequest request);

    Task<UpdateSlideResponse> UpdateSlideAsync(UpdateSlideRequest request);

    Task DeleteSlideAsync(DeleteSlideRequest request);
}
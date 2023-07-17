using miranaSolution.DTOs.Core.Chapters;

namespace miranaSolution.Services.Core.Chapters;

public interface IChapterService
{
    Task<CreateBookChapterResponse> CreateBookChapterAsync(CreateBookChapterRequest request);

    Task<GetAllBookChaptersResponse> GetAllBookChaptersAsync(GetAllBookChaptersRequest request);

    Task<GetBookChapterByIndexResponse> GetBookChapterByIndexAsync(GetBookChapterByIndexRequest request);

    Task<GetTotalBookChaptersResponse> GetTotalBookChaptersAsync(GetTotalBookChaptersRequest request);

    Task<GetLatestCreatedChaptersResponse> GetLatestCreatedChaptersAsync(GetLatestCreatedChaptersRequest request);
}
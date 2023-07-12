using miranaSolution.DTOs.Catalog.Chapters;

namespace miranaSolution.Services.Catalog.Chapters;

public interface IChapterService
{
    Task<CreateChapterResponse> CreateChapterAsync(CreateChapterRequest request);

    Task<GetAllChaptersResponse> GetAllChaptersAsync(GetAllChaptersRequest request);

    Task<GetChapterByIndexResponse> GetChapterByIndexAsync(GetChapterByIndexRequest request);

    Task<GetTotalChaptersResponse> GetTotalChaptersAsync(GetTotalChaptersRequest request);
}
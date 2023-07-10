using miranaSolution.DTOs.Catalog.Books.Chapters;
using miranaSolution.DTOs.Common;

namespace miranaSolution.Services.Catalog.Chapters;

public interface IChapterService
{
    Task<ChapterDto> AddChapter(int bookId, ChapterCreateRequest request);

    Task<PagedResult<ChapterDto>> GetChaptersPaging(int bookId, ChapterGetPagingRequest request);

    Task<ChapterDto> GetChapterByIndex(int bookId, int index);
}
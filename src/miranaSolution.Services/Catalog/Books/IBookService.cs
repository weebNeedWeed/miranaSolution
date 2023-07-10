using miranaSolution.DTOs.Catalog.Books;
using miranaSolution.DTOs.Catalog.Books.Chapters;
using miranaSolution.DTOs.Common;

namespace miranaSolution.Services.Catalog.Books;

public interface IBookService
{
    Task<PagedResult<BookDto>> GetPaging(BookGetPagingRequest request);

    Task<List<BookDto>> GetRecommended();

    Task<List<ChapterDto>> GetLatestChapters(int numOfChapters);

    Task<bool> Delete(int id);

    Task<bool> Update(int id, BookUpdateRequest request);

    Task<BookDto> Create(string userId, BookCreateRequest request);

    Task<BookDto> GetById(int id);

    Task<BookDto> GetBySlug(string slug);

    // Task<bool> UpdateChapter();
    //
    // Task<bool> DeleteChapter();
    //
    //
    // Task<ChapterDto> GetChapterById();
}
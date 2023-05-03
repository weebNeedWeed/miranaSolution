using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;

namespace miranaSolution.Business.Catalog.Books
{
    public interface IBookService
    {
        Task<PagedResult<BookDto>> GetPaging(BookGetPagingRequest request);

        Task<List<BookDto>> GetRecommended();

        Task<List<ChapterDto>> GetLatestChapters(int numOfChapters);

        Task<bool> Delete(int id);

        Task<bool> Update(int id, BookUpdateRequest request);

        Task<BookDto> Create(BookCreateRequest request);

        Task<BookDto> GetById(int id);

        Task<BookDto> GetBySlug(string slug);

        Task<ChapterDto> AddChapter(int id, ChapterCreateRequest request);

        // Task<bool> UpdateChapter();
        //
        // Task<bool> DeleteChapter();
        //
        // Task<ChapterDto> GetChapterByIndex();
        //
        // Task<ChapterDto> GetChapterById();
    }
}
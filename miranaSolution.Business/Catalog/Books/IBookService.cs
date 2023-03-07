using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;

namespace miranaSolution.Business.Catalog.Books
{
    public interface IBookService
    {
        Task<PagedResult<BookDto>> GetPaging(BookGetPagingRequest request);

        Task<List<BookDto>> GetRecommended();

        Task<bool> Delete(int id);

        Task<bool> Update(int id, BookUpdateRequest request);

        Task<BookDto> Create(BookCreateRequest request);

        Task<BookDto> GetById(int id);

        Task<BookDto> GetBySlug(string slug);
    }
}
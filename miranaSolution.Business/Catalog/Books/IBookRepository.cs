using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;

namespace miranaSolution.Business.Catalog.Books
{
    public interface IBookRepository
    {
        Task<PagedResult<BookDto>> GetPagingAsync(BookGetPagingRequest request);
    }
}
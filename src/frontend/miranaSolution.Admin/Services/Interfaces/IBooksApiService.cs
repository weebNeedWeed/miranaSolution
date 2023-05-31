using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IBooksApiService
{
    [Get("/books")]
    Task<ApiResult<PagedResult<BookDto>>> GetPaging(BookGetPagingRequest request);
}
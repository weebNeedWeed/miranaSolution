using miranaSolution.Dtos.Catalog.Authors;
using miranaSolution.Dtos.Common;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IAuthorsApiService
{
    [Get("/authors")]
    Task<ApiResult<List<AuthorDto>>> GetAll();
}
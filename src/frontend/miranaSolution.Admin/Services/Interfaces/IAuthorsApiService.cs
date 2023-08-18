using miranaSolution.API.ViewModels.Authors;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Core.Authors;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IAuthorsApiService
{
    [Get("/api/authors")]
    Task<ApiResult<ApiGetAllAuthorsResponse>> GetAllAuthorAsync();
    
    [Post("/api/authors")]
    Task<ApiResult<dynamic>> CreateAuthorAsync(ApiCreateAuthorRequest request);

    [Delete("/api/authors/{id}")]
    Task<ApiResult<dynamic>> DeleteAuthorAsync(
        [AliasAs("id")] int authorId);
    
    [Put("/api/authors/{id}")]
    Task<ApiResult<dynamic>> UpdateAuthorAsync(
        [AliasAs("id")] int authorId, [Body] ApiUpdateAuthorRequest request);
}
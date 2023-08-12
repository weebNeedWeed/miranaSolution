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
    Task<ApiResult<dynamic>> CreateAuthor(ApiCreateAuthorRequest request);
}
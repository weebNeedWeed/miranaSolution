using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Genres;
using miranaSolution.DTOs.Core.Genres;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IGenresApiService
{
    [Get("/api/genres")]
    Task<ApiResult<ApiGetAllGenreResponse>> GetAllGenresAsync();
    
    [Get("/api/genres/{id}")]
    Task<ApiResult<GenreVm>> GetGenreByIdAsync(
        [AliasAs("id")] int id);
    
    [Post("/api/genres")]
    Task<ApiResult<dynamic>> CreateGenreAsync(
        [Body] ApiCreateGenreRequest request);
    
    [Put("/api/genres/{id}")]
    Task<ApiResult<dynamic>> UpdateGenreAsync(
        [AliasAs("id")] int id,
        [Body] ApiUpdateGenreRequest request);
    
    [Delete("/api/genres/{id}")]
    Task<ApiResult<dynamic>> DeleteGenreAsync(
        [AliasAs("id")] int id);
}
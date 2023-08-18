using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Genres;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IGenresApiService
{
    [Get("/api/genres")]
    Task<ApiResult<ApiGetAllGenreResponse>> GetAllGenresAsync();
}
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Roles;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IRolesApiService
{
    [Get("/api/roles")]
    Task<ApiResult<ApiGetAllRolesResponse>> GetAllRolesAsync();
}
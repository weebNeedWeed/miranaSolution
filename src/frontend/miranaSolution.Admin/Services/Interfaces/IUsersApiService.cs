using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Users;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IUsersApiService
{
    [Get("/api/users")]
    Task<ApiResult<ApiGetAllUsersResponse>> GetAllUsersAsync(
        [Query]ApiGetAllUsersRequest request);

    [Delete("/api/users/{id}")]
    Task<ApiResult<dynamic>> DeleteUserAsync(
        [AliasAs("id")] string id);

    [Get("/api/users/{id}/roles")]
    Task<ApiResult<ApiGetRolesByUserIdResponse>> GetRolesByUserId(
        [AliasAs("id")] string id);
}
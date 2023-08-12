using miranaSolution.API.ViewModels.Authentication;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Authentication.Users;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IAuthApiService
{
    [Post("/auth/authenticate")]
    Task<ApiResult<ApiAuthenticateUserResponse>> AuthenticateUserAsync([Body] AuthenticateUserRequest request);
}
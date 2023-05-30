using miranaSolution.Dtos.Auth.Users;
using miranaSolution.Dtos.Common;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IUsersApiService
{
    [Post("/users/authenticate")]
    Task<ApiResult<dynamic>> Authenticate([Body] UserAuthenticationRequest request);
}
using miranaSolution.DTOs.Authentication.Roles;

namespace miranaSolution.Services.Authentication.Roles;

public interface IRoleService
{
    Task<GetAllRolesResponse> GetAllRolesAsync();
}
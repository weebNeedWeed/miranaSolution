using miranaSolution.DTOs.Authentication.Roles;
using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.API.ViewModels.Users;

public record ApiGetRolesByUserIdResponse(
    List<RoleVm> Roles);
using miranaSolution.DTOs.Authentication.Roles;

namespace miranaSolution.DTOs.Authentication.Users;

public record GetRolesByUserIdResponse(
    List<RoleVm> RoleVms);
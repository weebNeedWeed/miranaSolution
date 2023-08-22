using miranaSolution.DTOs.Authentication.Roles;

namespace miranaSolution.API.ViewModels.Roles;

public record ApiGetAllRolesResponse(
    List<RoleVm> Roles);
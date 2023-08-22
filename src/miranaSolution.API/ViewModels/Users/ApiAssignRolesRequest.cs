using miranaSolution.DTOs.Common;

namespace miranaSolution.API.ViewModels.Users;

public record ApiAssignRolesRequest(
    List<CheckboxItem> RoleCheckboxItems);
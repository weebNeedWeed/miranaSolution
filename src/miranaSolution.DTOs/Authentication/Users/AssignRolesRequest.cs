using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Authentication.Users;

public record AssignRolesRequest(
    Guid UserId,
    List<CheckboxItem> RoleCheckboxItems);
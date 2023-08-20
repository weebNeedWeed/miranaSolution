using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Authentication.Users;

public record GetAllUsersResponse(
    List<UserVm> UserVms,
    PagerResponse PagerResponse);
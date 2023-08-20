using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.API.ViewModels.Users;

public record ApiGetAllUsersResponse(
    List<UserVm> Users,
    int PageIndex,
    int PageSize,
    int TotalRecords,
    int TotalPages);
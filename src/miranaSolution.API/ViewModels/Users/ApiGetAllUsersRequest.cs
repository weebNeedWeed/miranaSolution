using Microsoft.AspNetCore.Mvc;

namespace miranaSolution.API.ViewModels.Users;

public record ApiGetAllUsersRequest(
    string? Keyword,
    int PageIndex = 1,
    int PageSize = 10);
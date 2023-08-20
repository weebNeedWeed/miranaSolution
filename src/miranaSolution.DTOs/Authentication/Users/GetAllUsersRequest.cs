using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Authentication.Users;

public record GetAllUsersRequest(
    string? Keyword,
    PagerRequest PagerRequest);
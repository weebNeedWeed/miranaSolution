namespace miranaSolution.API.ViewModels.Users;

public record ApiUpdateUserProfileResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? Avatar);
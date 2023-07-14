namespace miranaSolution.API.ViewModels.Users;

public record ApiUpdateUserPasswordResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? Avatar);
namespace miranaSolution.API.ViewModels.Users;

public record ApiAuthenticateUserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? Avatar,
    string Token);
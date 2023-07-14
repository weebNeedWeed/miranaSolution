namespace miranaSolution.API.ViewModels.Users;

public record ApiUpdateUserInformationResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? Avatar);
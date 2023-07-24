namespace miranaSolution.API.ViewModels.Users;

public record ApiUpdateUserProfileRequest(
    string FirstName,
    string LastName,
    string Email,
    IFormFile? Avatar);
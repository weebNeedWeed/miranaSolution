namespace miranaSolution.API.ViewModels.Users;

public record ApiUpdateUserInformationRequest(
    string FirstName,
    string LastName,
    string Email,
    IFormFile? Avatar);
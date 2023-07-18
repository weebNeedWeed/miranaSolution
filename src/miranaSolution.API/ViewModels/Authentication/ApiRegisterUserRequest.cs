namespace miranaSolution.API.ViewModels.Authentication;

public record ApiRegisterUserRequest(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    string PasswordConfirmation);
namespace miranaSolution.API.ViewModels.Users;

public record ApiRegisterUserRequest(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    string PasswordConfirmation);
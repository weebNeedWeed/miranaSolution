namespace miranaSolution.API.ViewModels.Users;

public record ApiAuthenticateUserRequest(
    string UserName,
    string Password);
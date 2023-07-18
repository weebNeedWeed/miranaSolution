namespace miranaSolution.API.ViewModels.Authentication;

public record ApiAuthenticateUserRequest(
    string UserName,
    string Password);
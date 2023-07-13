namespace miranaSolution.DTOs.Authentication.Users;

public record AuthenticateUserRequest(
    string UserName,
    string Password);
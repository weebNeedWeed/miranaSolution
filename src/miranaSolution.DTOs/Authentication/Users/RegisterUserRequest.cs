namespace miranaSolution.DTOs.Authentication.Users;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password);
namespace miranaSolution.DTOs.Authentication.Users;

public record UpdateUserPasswordRequest(
    string UserName, 
    string CurrentPassword,
    string NewPassword);
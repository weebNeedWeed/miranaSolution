namespace miranaSolution.DTOs.Authentication.Users;

public record UpdateUserProfileRequest(
    string UserName,
    string FirstName,
    string LastName,
    string Email,
    Stream? Avatar,
    string? AvatarExtension);
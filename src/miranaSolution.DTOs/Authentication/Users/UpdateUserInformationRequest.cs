namespace miranaSolution.DTOs.Authentication.Users;

public record UpdateUserInformationRequest(
    string UserName,
    string FirstName,
    string LastName,
    string Email,
    Stream? Avatar,
    string? AvatarExtension);
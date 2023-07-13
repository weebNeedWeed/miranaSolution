namespace miranaSolution.DTOs.Authentication.Users;

public record AuthenticateUserResponse(
    UserVm UserVm,
    string Token);
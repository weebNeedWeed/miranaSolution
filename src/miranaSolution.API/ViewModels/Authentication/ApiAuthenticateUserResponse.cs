using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.API.ViewModels.Authentication;

public record ApiAuthenticateUserResponse(
    UserVm User,
    string Token);
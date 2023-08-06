namespace miranaSolution.API.ViewModels.Authentication;

public record ApiResetPasswordRequest(
    string Email,
    string Callback);
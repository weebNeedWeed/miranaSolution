namespace miranaSolution.API.ViewModels.Authentication;

public record ApiRedeemTokenRequest(
    string Token,
    string Email,
    string NewPassword,
    string NewPasswordConfirmation);
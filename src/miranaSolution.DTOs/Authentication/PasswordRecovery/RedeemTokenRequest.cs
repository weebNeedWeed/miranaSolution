namespace miranaSolution.DTOs.Authentication.PasswordRecovery;

public record RedeemTokenRequest(
    string Token,
    string Email,
    string NewPassword,
    string NewPasswordConfirmation);
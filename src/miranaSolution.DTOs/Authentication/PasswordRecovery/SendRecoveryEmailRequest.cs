namespace miranaSolution.DTOs.Authentication.PasswordRecovery;

public record SendRecoveryEmailRequest(
    string Email,
    string Callback);
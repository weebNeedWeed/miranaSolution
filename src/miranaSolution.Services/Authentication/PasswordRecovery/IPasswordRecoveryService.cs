using miranaSolution.DTOs.Authentication.PasswordRecovery;

namespace miranaSolution.Services.Authentication.PasswordRecovery;

public interface IPasswordRecoveryService
{
    Task SendRecoveryEmailAsync(SendRecoveryEmailRequest request);

    Task RedeemTokenAsync(RedeemTokenRequest request);
}
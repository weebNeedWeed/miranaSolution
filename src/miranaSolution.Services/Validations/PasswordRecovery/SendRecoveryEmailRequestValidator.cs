using FluentValidation;
using miranaSolution.DTOs.Authentication.PasswordRecovery;

namespace miranaSolution.Services.Validations.PasswordRecovery;

public class SendRecoveryEmailRequestValidator : AbstractValidator<SendRecoveryEmailRequest>
{
    public SendRecoveryEmailRequestValidator()
    {
        RuleFor(x => x.Callback)
            .NotEmpty().WithMessage("Callback không được để trống.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Phải có dạng email.");
    }
}
using FluentValidation;
using miranaSolution.DTOs.Authentication.PasswordRecovery;

namespace miranaSolution.Services.Validations.PasswordRecovery;

public class RedeemTokenRequestValidator : AbstractValidator<RedeemTokenRequest>
{
    public RedeemTokenRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Phải có dạng email.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token không được để trống.");

        RuleFor(p => p.NewPassword)
            .NotEmpty()
            .WithMessage("Mật khẩu không được trống.")
            .MinimumLength(8)
            .WithMessage("Mật khẩu phải có độ dài bé hơn 32 ký tự.")
            .MaximumLength(32)
            .WithMessage("Mật khẩu phải có độ dài lớn hơn 8 ký tự.")
            .Matches(@"[A-Z]+").WithMessage("Mật khẩu phải gồm 1 ký tự hoa.")
            .Matches(@"[a-z]+").WithMessage("Mật khẩu phải gồm 1 ký tự thuờng.")
            .Matches(@"[0-9]+").WithMessage("Mật khẩu phải gồm 1 ký tự số.")
            .Matches(@"[\!\?\*\.\@]+").WithMessage("Mật khẩu phải gồm 1 ký tự đặc biệt (!? *.@).");
        
        RuleFor(x => x.NewPasswordConfirmation)
            .NotEmpty()
            .WithMessage("Vui lòng nhập lại mật khẩu.")
            .Equal(x => x.NewPassword)
            .When(x => !string.IsNullOrEmpty(x.NewPassword))
            .WithMessage("Mật khẩu không trùng khớp.");
    }
}
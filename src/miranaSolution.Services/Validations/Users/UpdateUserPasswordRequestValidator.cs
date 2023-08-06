using FluentValidation;
using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.Services.Validations.Users;

public class UpdateUserPasswordRequestValidator : AbstractValidator<UpdateUserPasswordRequest>
{
    public UpdateUserPasswordRequestValidator()
    {
        RuleFor(p => p.NewPassword)
            .NotEmpty()
            .WithMessage("Mật khẩu không được trống.")
            .Length(8, 32)
            .WithMessage("Mật khẩu phải có độ dài từ 8 tới 32 ký tự.")
            .Matches(@"[A-Z]+").WithMessage("Mật khẩu phải gồm 1 ký tự hoa.")
            .Matches(@"[a-z]+").WithMessage("Mật khẩu phải gồm 1 ký tự thuờng.")
            .Matches(@"[0-9]+").WithMessage("Mật khẩu phải gồm 1 ký tự số.")
            .Matches(@"[\!\?\*\.\@]+").WithMessage("Mật khẩu phải gồm 1 ký tự đặc biệt (!? *.@).");

        // Only validation when password is not null or empty
        RuleFor(x => x.NewPasswordConfirmation)
            .NotEmpty()
            .WithMessage("Vui lòng nhập lại mật khẩu.")
            .Equal(x => x.NewPassword)
            .When(x => !string.IsNullOrEmpty(x.NewPassword))
            .WithMessage("Mật khẩu không trùng khớp.");
    }
}
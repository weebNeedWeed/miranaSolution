using FluentValidation;
using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.Services.Validations.Users;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Tên không được trống.")
            .Length(1, 16)
            .WithMessage("Tên phải có độ dài từ 1 tới 16 ký tự.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Họ không được trống.")
            .Length(1, 16)
            .WithMessage("Họ phải có độ dài từ 1 tới 16 ký tự.");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Tài khoản không được trống.")
            .Length(8, 32)
            .WithMessage("Tài khoản phải có độ dài từ 8 tới 32 ký tự.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email không được trống.")
            .EmailAddress()
            .WithMessage("Phải là email.");

        RuleFor(p => p.Password)
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

        // Only validation when password is not null or empty
        RuleFor(x => x.PasswordConfirmation)
            .NotEmpty()
            .WithMessage("Vui lòng nhập lại mật khẩu.")
            .Equal(x => x.Password)
            .When(x => !string.IsNullOrEmpty(x.Password))
            .WithMessage("Mật khẩu không trùng khớp.");
    }
}
using FluentValidation;
using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.Services.Validations.Users;

public class AuthenticateUserRequestValidator : AbstractValidator<AuthenticateUserRequest>
{
    public AuthenticateUserRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Tài khoản không được trống.");

        RuleFor(p => p.Password)
            .NotEmpty()
            .WithMessage("Mật khẩu không được trống.");
    }
}
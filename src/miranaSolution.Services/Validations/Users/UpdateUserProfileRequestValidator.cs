using FluentValidation;
using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.Services.Validations.Users;

public class UpdateUserProfileRequestValidator : AbstractValidator<UpdateUserProfileRequest>
{
    public UpdateUserProfileRequestValidator()
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
    }
}
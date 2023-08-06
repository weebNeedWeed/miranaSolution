using FluentValidation;
using miranaSolution.DTOs.Core.Authors;

namespace miranaSolution.Services.Validations.Authors;

public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
{
    public UpdateAuthorRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống.")
            .Length(4, 128)
            .WithMessage("Name phải có độ dài từ 4 tới 128 ký tự.");
    }
}
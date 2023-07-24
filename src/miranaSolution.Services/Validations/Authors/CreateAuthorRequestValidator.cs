using FluentValidation;
using miranaSolution.DTOs.Core.Authors;

namespace miranaSolution.Services.Validations.Authors;

public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
{
    public CreateAuthorRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống.")
            .Length(min: 4, max: 128)
            .WithMessage("Name phải có độ dài từ 4 tới 128 ký tự.");
    }
}
using FluentValidation;
using miranaSolution.DTOs.Core.Genres;

namespace miranaSolution.Services.Validations.Genres;

public class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequest>
{
    public CreateGenreRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống.")
            .Length(8, 128)
            .WithMessage("Name phải có độ dài từ 8 tới 128 ký tự.");
    }
}
using FluentValidation;
using miranaSolution.DTOs.Core.Genres;

namespace miranaSolution.Services.Validations.Genres;

public class UpdateGenreRequestValidator : AbstractValidator<UpdateGenreRequest>
{
    public UpdateGenreRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống.")
            .Length(2, 128)
            .WithMessage("Name phải có độ dài từ 2 tới 128 ký tự.");
    }
}
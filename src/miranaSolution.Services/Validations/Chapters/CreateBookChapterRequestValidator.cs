using FluentValidation;
using miranaSolution.DTOs.Core.Chapters;

namespace miranaSolution.Services.Validations.Chapters;

public class CreateBookChapterRequestValidator : AbstractValidator<CreateBookChapterRequest>
{
    public CreateBookChapterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống.")
            .Length(2, 256)
            .WithMessage("Name phải có độ dài từ 2 tới 256 ký tự.");

        RuleFor(x => x.WordCount)
            .NotNull()
            .WithMessage("WordCount không được để trống.")
            .GreaterThan(0)
            .WithMessage("WordCount phải lớn hơn 0.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content không được để trống.")
            .Length(8, 32768)
            .WithMessage("Content phải có độ dài từ 8 tới 32768 ký tự.");
        
        RuleFor(x => x.Index)
            .NotNull()
            .WithMessage("Index không được để trống.")
            .GreaterThan(0)
            .WithMessage("Index phải lớn hơn 0.");
    }
}
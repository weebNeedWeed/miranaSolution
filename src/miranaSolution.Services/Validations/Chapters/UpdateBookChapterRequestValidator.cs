using FluentValidation;
using miranaSolution.DTOs.Core.Chapters;

namespace miranaSolution.Services.Validations.Chapters;

public class UpdateBookChapterRequestValidator : AbstractValidator<UpdateBookChapterRequest>
{
    public UpdateBookChapterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống.")
            .Length(8, 32)
            .WithMessage("Name phải có độ dài từ 8 tới 32 ký tự.");

        RuleFor(x => x.WordCount)
            .NotNull()
            .WithMessage("WordCount không được để trống.")
            .GreaterThan(0)
            .WithMessage("WordCount phải lớn hơn 0.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content không được để trống.")
            .Length(8, 8192)
            .WithMessage("Content phải có độ dài từ 8 tới 8192 ký tự.");
        
        RuleFor(x => x.NewIndex)
            .NotNull()
            .WithMessage("Index không được để trống.")
            .GreaterThan(0)
            .WithMessage("Index phải lớn hơn 0.");
    }
}
using FluentValidation;
using miranaSolution.DTOs.Core.Slides;

namespace miranaSolution.Services.Validations.Slides;

public class UpdateSlideRequestValidator : AbstractValidator<UpdateSlideRequest>
{
    public UpdateSlideRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống.")
            .Length(2, 256)
            .WithMessage("Name phải có độ dài từ 2 tới 256 ký tự.");
        
        RuleFor(x => x.ShortDescription)
            .NotNull()
            .WithMessage("ShortDescription không được để trống.")
            .Length(0, 512)
            .WithMessage("ShortDescription phải có độ dài từ 0 tới 512 ký tự.");
        
        RuleFor(x => x.Genres)
            .NotNull()
            .WithMessage("Genres không được để trống.")
            .Length(0, 512)
            .WithMessage("Genres phải có độ dài từ 0 tới 512 ký tự.");

        RuleFor(x => x.SortOrder)
            .NotNull()
            .WithMessage("SortOrder không được để trống.")
            .GreaterThan(0)
            .WithMessage("SortOrder phải lớn hơn 0.");
        
        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage("Url không được để trống.");
    }
}
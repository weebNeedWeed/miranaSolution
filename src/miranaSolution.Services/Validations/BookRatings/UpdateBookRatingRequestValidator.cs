using FluentValidation;
using miranaSolution.DTOs.Core.BookRatings;

namespace miranaSolution.Services.Validations.BookRatings;

public class UpdateBookRatingRequestValidator : AbstractValidator<UpdateBookRatingRequest>
{
    public UpdateBookRatingRequestValidator()
    {
        RuleFor(x => x.Content)
            .MaximumLength(256)
            .WithMessage("Content phải có độ dài từ 1 tới 256 ký tự.");

        RuleFor(x => x.Star)
            .NotEmpty()
            .WithMessage("Star không được để trống.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Star phải lớn hơn bằng 1.")
            .LessThanOrEqualTo(5)
            .WithMessage("Star phải bé hơn bằng 5.");
    }
}
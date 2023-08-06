using FluentValidation;
using miranaSolution.DTOs.Core.Comments;

namespace miranaSolution.Services.Validations.Comments;

public class CreateBookCommentRequestValidator : AbstractValidator<CreateBookCommentRequest>
{
    public CreateBookCommentRequestValidator()
    {
        RuleFor(x => x.ParentId)
            .GreaterThan(0)
            .WithMessage("ParentId phải lớn hơn 0.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content không được để trống.")
            .Length(1, 256)
            .WithMessage("Content phải có độ dài từ 1 tới 256 ký tự.");
    }
}
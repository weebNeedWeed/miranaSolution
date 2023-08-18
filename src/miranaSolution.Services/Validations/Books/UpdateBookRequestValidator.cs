using FluentValidation;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.Services.Validations.Common;

namespace miranaSolution.Services.Validations.Books;

public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
{
    public UpdateBookRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống.")
            .Length(8, 128)
            .WithMessage("Name phải có độ dài từ 8 tới 128 ký tự.");

        RuleFor(x => x.IsRecommended)
            .NotNull()
            .WithMessage("IsRecommended không được để trống.");

        RuleFor(x => x.ShortDescription)
            .NotNull()
            .WithMessage("ShortDescription không được để trống.")
            .Length(0, 256)
            .WithMessage("ShortDescription phải có độ dài từ 0 tới 256 ký tự.");

        RuleFor(x => x.LongDescription)
            .NotNull()
            .WithMessage("LongDescription không được để trống.")
            .Length(0, 512)
            .WithMessage("LongDescription phải có độ dài từ 0 tới 512 ký tự.");

        RuleFor(x => x.IsDone)
            .NotNull()
            .WithMessage("IsDone không được để trống.");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Slug không được để trống.")
            .Length(8, 128)
            .WithMessage("Slug phải có độ dài từ 8 tới 128 ký tự.");

        RuleFor(x => x.AuthorId)
            .NotNull()
            .WithMessage("AuthorId không được để trống.");
    }
}
﻿using FluentValidation;
using miranaSolution.DTOs.Core.Slides;

namespace miranaSolution.Services.Validations.Slides;

public class CreateSlideRequestValidator : AbstractValidator<CreateSlideRequest>
{
    public CreateSlideRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name không được để trống.")
            .Length(8, 32)
            .WithMessage("Name phải có độ dài từ 8 tới 128 ký tự.");
        
        RuleFor(x => x.ShortDescription)
            .NotNull()
            .WithMessage("ShortDescription không được để trống.")
            .Length(0, 256)
            .WithMessage("ShortDescription phải có độ dài từ 0 tới 256 ký tự.");
        
        RuleFor(x => x.Genres)
            .NotNull()
            .WithMessage("Genres không được để trống.")
            .Length(0, 128)
            .WithMessage("Genres phải có độ dài từ 0 tới 128 ký tự.");

        RuleFor(x => x.SortOrder)
            .NotNull()
            .WithMessage("SortOrder không được để trống.")
            .GreaterThan(0)
            .WithMessage("SortOrder phải lớn hơn 0.");

        RuleFor(x => x.ThumbnailImage)
            .NotNull()
            .WithMessage("ThumbnailImage không được để trống.");
    }
}
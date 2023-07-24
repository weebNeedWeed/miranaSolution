using FluentValidation;
using miranaSolution.DTOs.Common;

namespace miranaSolution.Services.Validations.Common;

public class CheckboxItemValidator : AbstractValidator<CheckboxItem>
{
    public CheckboxItemValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.Label)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.IsChecked)
            .NotNull();
    }
}
using FluentValidation;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.Services.Validations.Common;

namespace miranaSolution.Services.Validations.Books;

public class AssignGenresRequestValidator : AbstractValidator<AssignGenresRequest>
{
    public AssignGenresRequestValidator()
    {
        RuleForEach(x => x.GenreCheckboxItems)
            .NotNull()
            .SetValidator(new CheckboxItemValidator());
    }
}
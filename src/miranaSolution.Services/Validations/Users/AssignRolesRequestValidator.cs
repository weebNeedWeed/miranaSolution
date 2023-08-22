using FluentValidation;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.Services.Validations.Common;

namespace miranaSolution.Services.Validations.Users;

public class AssignRolesRequestValidator : AbstractValidator<AssignRolesRequest>
{
    public AssignRolesRequestValidator()
    {
        RuleForEach(x => x.RoleCheckboxItems)
            .NotNull()
            .SetValidator(new CheckboxItemValidator());
    }
}
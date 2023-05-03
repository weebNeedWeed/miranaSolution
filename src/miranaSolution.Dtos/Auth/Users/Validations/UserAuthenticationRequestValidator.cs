using FluentValidation;

namespace miranaSolution.Dtos.Auth.Users.Validations
{
    public class UserAuthenticationRequestValidator : AbstractValidator<UserAuthenticationRequest>
    {
        public UserAuthenticationRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(8);

            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);

            RuleFor(x => x.RememberMe).NotNull();
        }
    }
}
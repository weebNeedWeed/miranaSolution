using FluentValidation;
using FluentValidation.Results;

namespace miranaSolution.Services.Validations;

public class ValidatorProvider : IValidatorProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ValidatorProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Validate(object obj)
    {
        var neededType = typeof(IValidator<>).MakeGenericType(obj.GetType());
        var validator = _serviceProvider.GetService(neededType);
        if (validator is null) throw new NullReferenceException("The validator for the input object is not injected.");

        var method = validator.GetType()
            .GetMethod("Validate", new[] { obj.GetType() })!;

        var validationResult = (ValidationResult)method
            .Invoke(validator, new[] { obj })!;

        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
    }

    private dynamic Cast(dynamic obj, Type dest)
    {
        return Convert.ChangeType(obj, dest);
    }
}
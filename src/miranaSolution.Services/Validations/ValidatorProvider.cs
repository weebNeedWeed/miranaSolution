using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace miranaSolution.Services.Validations;

public class ValidatorProvider : IValidatorProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ValidatorProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private dynamic Cast(dynamic obj, Type dest)
    {
        return Convert.ChangeType(obj, dest);
    }
    
    public void Validate(object obj)
    {
        var neededType = typeof(IValidator<>).MakeGenericType(obj.GetType());
        var validator = _serviceProvider.GetService(neededType);
        if (validator is null)
        {
            throw new NullReferenceException("The validator for the object is not injected.");
        }

        var method = validator.GetType()
            .GetMethod("Validate", new Type[] { obj.GetType() })!;

        var validationResult = (ValidationResult)method
            .Invoke(validator, new[] { obj })!;

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}
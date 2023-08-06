using FluentValidation.Results;
using miranaSolution.API.ViewModels.Common;

namespace miranaSolution.API.Extensions;

public static class ValidationFailureListExtensions
{
    public static ApiFailResult ToApiFailResult(this IEnumerable<ValidationFailure> errors)
    {
        var dict = new Dictionary<string, string>();
        foreach (var error in errors)
        {
            var firstLetterLowercasePropertyName =
                $"{char.ToLower(error.PropertyName[0])}{error.PropertyName[1..]}";

            dict[firstLetterLowercasePropertyName] = error.ErrorMessage;
        }

        return new ApiFailResult(dict);
    }
}
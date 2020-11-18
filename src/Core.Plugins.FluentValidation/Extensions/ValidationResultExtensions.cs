using FluentValidation;
using FluentValidation.Results;
using System;
using System.Linq;

namespace Core.Plugins.FluentValidation.Extensions
{
    public static class ValidationResultExtensions
    {
        public static void ThrowOnFailure(this ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                string errorMessage = $"Validation Failure: The following validation failures occurred: {Environment.NewLine}";

                errorMessage += string.Join(Environment.NewLine, validationResult.Errors.Select(failure => $"'{failure.ErrorMessage}'"));

                throw new ValidationException(errorMessage);
            }
        }
    }
}

using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Plugins.FluentValidation.Extensions
{
    public static class ValidatorExtensions
    {
        public static async Task ValidateAndThrowAsync<TToValidate>(this IValidator<TToValidate> validator, TToValidate instance, CancellationToken cancellationToken)
        {
            ValidationResult result = await validator.ValidateAsync(instance, cancellationToken);

            result.ThrowOnFailure();
        }
    }
}

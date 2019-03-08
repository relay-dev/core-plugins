using System;
using System.Collections.Generic;
using System.Linq;
using Core.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace Core.Plugins.FluentValidation.Base
{
    public class FluentValidationBase<TToValidate> : AbstractValidator<TToValidate>, Validation.IValidator<TToValidate>
    {
        public ValidatorResult Run(TToValidate instance)
        {
            return Run(instance, null);
        }

        public ValidatorResult Run(TToValidate instance, string ruleSet)
        {
            ValidationResult validationResult = String.IsNullOrEmpty(ruleSet)
               ? Validate(instance)
               : this.Validate(instance, ruleSet: ruleSet);

            if (validationResult.IsValid)
            {
                return new ValidatorResult();
            }

            string typeName = typeof(TToValidate).Name;

            IEnumerable<ValidatorFailure> validatorFailures = validationResult.Errors
                .Select(error => new ValidatorFailure(typeName, error.PropertyName, error.ErrorMessage));

            return new ValidatorResult(validatorFailures);
        }
    }
}

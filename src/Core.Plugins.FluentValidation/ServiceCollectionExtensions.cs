using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.FluentValidation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentValidationPlugin(this IServiceCollection services, dynamic mvcCoreBuilder, Dictionary<Type, Type> validatorTypes)
        {
            if (validatorTypes == null || !validatorTypes.Any())
            {
                return services;
            }

            // Add FluentValidation
            mvcCoreBuilder.AddFluentValidation();

            // Add the validators
            foreach (KeyValuePair<Type, Type> validatorType in validatorTypes)
            {
                services.AddTransient(validatorType.Key, validatorType.Value);
            }

            return services;
        }
    }
}

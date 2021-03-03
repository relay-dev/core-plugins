using Core.Plugins.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.FluentValidation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentValidationPlugin(this IServiceCollection services, PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration.ValidatorTypes == null || !pluginConfiguration.ValidatorTypes.Any())
            {
                return services;
            }

            // Add the validators
            foreach (KeyValuePair<Type, Type> validatorType in pluginConfiguration.ValidatorTypes)
            {
                services.AddTransient(validatorType.Key, validatorType.Value);
            }

            return services;
        }

        public static IServiceCollection AddFluentValidationPlugin(this IServiceCollection services, dynamic mvcCoreBuilder, PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration.ValidatorTypes == null || !pluginConfiguration.ValidatorTypes.Any())
            {
                return services;
            }

            // Add FluentValidation
            mvcCoreBuilder.AddFluentValidation();

            // Add the validators
            foreach (KeyValuePair<Type, Type> validatorType in pluginConfiguration.ValidatorTypes)
            {
                services.AddTransient(validatorType.Key, validatorType.Value);
            }

            return services;
        }
    }
}

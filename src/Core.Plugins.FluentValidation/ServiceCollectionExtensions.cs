using Core.Plugins.Configuration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Core.Plugins.FluentValidation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentValidationPlugin(this IServiceCollection services, PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration.ValidatorAssemblies != null && pluginConfiguration.ValidatorAssemblies.Any())
            {
                foreach (Assembly assembly in pluginConfiguration.ValidatorAssemblies)
                {
                    assembly.GetTypes()
                        .Where(t => t.GetInterfaces().Contains(typeof(IValidator))).ToList()
                        .ForEach(t =>
                        {
                            if (t.BaseType != null && t.BaseType.GetGenericArguments().Length == 1)
                            {
                                Type validatorOf = t.BaseType.GetGenericArguments()[0];
                                Type serviceType = typeof(IValidator<>).MakeGenericType(validatorOf);
                                services.AddTransient(serviceType, t);
                            }
                        });
                }
            }

            if (pluginConfiguration.ValidatorTypes != null && pluginConfiguration.ValidatorTypes.Any())
            {
                foreach (var validatorType in pluginConfiguration.ValidatorTypes)
                {
                    services.AddTransient(validatorType.Key, validatorType.Value);
                }
            }

            return services;
        }
    }
}

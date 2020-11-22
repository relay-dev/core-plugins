using AutoMapper;
using Core.Plugins.AutoMapper.Extensions;
using Core.Plugins.AutoMapper.Resolvers.Database;
using Core.Plugins.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Core.Plugins.AutoMapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapperPlugin(this IServiceCollection services, PluginConfiguration pluginConfiguration)
        {
            if (!pluginConfiguration.MapperTypes.Any() && !pluginConfiguration.MapperAssemblies.Any())
            {
                return services;
            }

            // Discover mappers
            if (pluginConfiguration.MapperAssemblies.Any())
            {
                foreach (Type type in pluginConfiguration.MapperAssemblies.SelectMany(a => a.GetTypes()))
                {
                    if (type.IsSubclassOf(typeof(Profile)))
                    {
                        pluginConfiguration.MapperTypes.Add(type);
                    }
                }
            }

            // Add AutoMapper
            services
                .AddAutoMapper(cfg =>
                {
                    services.AddSingleton(provider =>
                    {
                        cfg.ConstructServicesUsing(type => ActivatorUtilities.CreateInstance(provider, type));

                        return cfg;
                    });

                    cfg.AddCoreAutoMappers();
                }, pluginConfiguration.MapperTypes);

            // Add Resolvers
            services.AddScoped(typeof(LookupDataKeyResolver<>));
            services.AddScoped(typeof(LookupDataValueResolver<>));

            return services;
        }
    }
}

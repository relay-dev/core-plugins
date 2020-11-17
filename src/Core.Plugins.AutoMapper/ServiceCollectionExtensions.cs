using AutoMapper;
using Core.Plugins.AutoMapper.Extensions;
using Core.Plugins.AutoMapper.Resolvers.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.AutoMapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapperPlugin(this IServiceCollection services, List<Type> mapperTypes)
        {
            if (mapperTypes == null || !mapperTypes.Any())
            {
                return services;
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
                });

            // Add Resolvers
            services.AddScoped(typeof(LookupDataKeyResolver<>));
            services.AddScoped(typeof(LookupDataValueResolver<>));

            return services;
        }
    }
}

using AutoMapper;
using Core.Plugins.AutoMapper.Mappers;
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
                }, mapperTypes.Union(CoreMapperTypes));

            // Add Resolvers
            services.AddScoped(typeof(LookupDataKeyResolver<>));
            services.AddScoped(typeof(LookupDataValueResolver<>));

            return services;
        }

        private static readonly List<Type> CoreMapperTypes = new List<Type>
        {
            typeof(PrimitiveMappers),
            typeof(SystemMappers)
        };
    }
}

using Core.Application;
using Core.Framework;
using Core.Plugins.Application;
using Core.Plugins.Configuration;
using Core.Plugins.Framework;
using Core.Plugins.Providers;
using Core.Plugins.Utilities;
using Core.Plugins.Validation;
using Core.Providers;
using Core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Plugins
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, ApplicationConfiguration applicationConfiguration)
        {
            // Add Configuration
            services.AddSingleton(applicationConfiguration);
            services.AddSingleton(applicationConfiguration.Configuration);
            services.AddSingleton(applicationConfiguration.ApplicationContext);

            return services;
        }

        public static IServiceCollection AddCorePlugins(this IServiceCollection services, PluginConfiguration pluginConfiguration)
        {
            // Add Application services
            services.AddScoped<IConnectionStringParser, ConnectionStringParser>();

            // Add Providers
            services.AddScoped<IApplicationContextProvider, ApplicationContextProvider>();
            services.AddScoped<ICommandContextProvider, CommandContextProvider>();
            services.AddScoped<IConnectionStringProvider, ConnectionStringByConfigurationProvider>();
            services.AddScoped<IDateTimeProvider, DateTimeUtcProvider>();
            services.AddScoped<IGuidProvider, GuidProvider>();
            services.AddScoped<IKeyProvider, EncryptionKeyProvider>();
            services.AddScoped<IRandomCodeProvider, RandomCodeProvider>();
            services.AddScoped<IRandomLongProvider, RandomLongProvider>();
            services.AddScoped<IResourceProvider, ResourceProvider>();
            services.AddScoped<ISequenceProvider, SequenceProvider>();
            services.AddScoped<IUsernameProvider, UsernameProvider>();

            // Add Framework
            services.AddScoped<IWarmupTaskExecutor, WarmupTaskExecutor>();

            // Add Utilities
            services.AddScoped<IAssemblyScanner, AssemblyScanner>();
            services.AddScoped<IInlineValidator, InlineValidator>();

            // Add MemoryCache
            services.AddMemoryCache();

            // Add Configuration
            services.AddSingleton(pluginConfiguration);

            return services;
        }

        public static IServiceCollection AddTypesWithAttribute<TAttribute>(this IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<TAttribute, bool> predicate = null) where TAttribute : Attribute
        {
            var assemblies = new List<Assembly> { assembly };

            return services.AddTypesWithAttribute(assemblies, serviceLifetime, predicate);
        }

        public static IServiceCollection AddTypesWithAttribute<TAttribute>(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<TAttribute, bool> predicate = null) where TAttribute : Attribute
        {
            IEnumerable<Type> types = AssemblyScanner.Instance.FindTypesWithAttribute(assemblies, predicate);

            return services.AddTypes(types, serviceLifetime);
        }

        public static IServiceCollection AddTypesWithBaseClass<TBaseClass>(this IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<Type, bool> predicate = null)
        {
            var assemblies = new List<Assembly> { assembly };

            return services.AddTypesWithBaseClass<TBaseClass>(assemblies, serviceLifetime, predicate);
        }

        public static IServiceCollection AddTypesWithBaseClass<TBaseClass>(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<Type, bool> predicate = null)
        {
            IEnumerable<Type> types = AssemblyScanner.Instance.FindTypesWithBaseClass<TBaseClass>(assemblies, predicate);

            return services.AddTypes(types, serviceLifetime);
        }

        public static IServiceCollection AddTypesWithInterface<TInterface>(this IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<Type, bool> predicate = null)
        {
            var assemblies = new List<Assembly> { assembly };

            return services.AddTypesWithInterface<TInterface>(assemblies, serviceLifetime, predicate);
        }

        public static IServiceCollection AddTypesWithInterface<TInterface>(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<Type, bool> predicate = null)
        {
            IEnumerable<Type> types = AssemblyScanner.Instance.FindTypesWithInterface<TInterface>(assemblies, predicate);

            return services.AddTypes(types, serviceLifetime);
        }

        public static IServiceCollection AddTypes(this IServiceCollection services, IEnumerable<Type> types, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            foreach (Type type in types)
            {
                var serviceDescriptor = new ServiceDescriptor(type, type, serviceLifetime);

                services.Add(serviceDescriptor);
            }

            return services;
        }

        public static IServiceCollection AddWarmup(this IServiceCollection services, PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration.WarmupTypes == null || !pluginConfiguration.WarmupTypes.Any())
            {
                return services;
            }

            pluginConfiguration.WarmupTypes.ForEach(warmupType =>
            {
                services.AddTransient(warmupType);
            });

            return services;
        }
    }
}

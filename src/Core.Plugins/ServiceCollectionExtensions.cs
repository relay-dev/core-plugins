using Core.Application;
using Core.Framework;
using Core.Plugins.Application;
using Core.Plugins.Configuration;
using Core.Plugins.Framework;
using Core.Plugins.Providers;
using Core.Plugins.Utilities;
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
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, ApplicationConfiguration applicationConfiguration)
        {
            // Add Types from Attribute
            if (applicationConfiguration.TypesToRegisterFromAttribute.Any())
            {
                foreach (var kvp in applicationConfiguration.TypesToRegisterFromAttribute)
                {
                    services.AddTypesWithAttribute(kvp.Key, kvp.Value, applicationConfiguration.ServiceLifetime);
                }
            }

            // Add Types from Base Type
            if (applicationConfiguration.TypesToRegisterFromBaseType.Any())
            {
                foreach (var kvp in applicationConfiguration.TypesToRegisterFromBaseType)
                {
                    services.AddTypesWithBaseClass(kvp.Key, kvp.Value, applicationConfiguration.ServiceLifetime);
                }
            }

            // Add Types from Interface
            if (applicationConfiguration.TypesToRegisterFromInterface.Any())
            {
                foreach (var kvp in applicationConfiguration.TypesToRegisterFromInterface)
                {
                    services.AddTypesWithInterface(kvp.Key, kvp.Value, applicationConfiguration.ServiceLifetime);
                }
            }

            // Add Configuration
            services.AddSingleton(applicationConfiguration);
            services.AddSingleton(applicationConfiguration.Configuration);
            services.AddSingleton(applicationConfiguration.ApplicationContext);

            return services;
        }

        public static IServiceCollection AddCorePlugins(this IServiceCollection services, PluginConfiguration pluginConfiguration)
        {
            // Add Application services
            services.Add<IConnectionStringParser, ConnectionStringParser>(pluginConfiguration.ServiceLifetime);

            // Add Providers
            services.Add<IApplicationContextProvider, ApplicationContextProvider>(pluginConfiguration.ServiceLifetime);
            services.Add<IConnectionStringProvider, ConnectionStringByConfigurationProvider>(pluginConfiguration.ServiceLifetime);
            services.Add<IDateTimeProvider, DateTimeUtcProvider>(pluginConfiguration.ServiceLifetime);
            services.Add<IGuidProvider, GuidProvider>(pluginConfiguration.ServiceLifetime);
            services.Add<IKeyProvider, EncryptionKeyProvider>(pluginConfiguration.ServiceLifetime);
            services.Add<IRandomCodeProvider, RandomCodeProvider>(pluginConfiguration.ServiceLifetime);
            services.Add<IRandomLongProvider, RandomLongProvider>(pluginConfiguration.ServiceLifetime);
            services.Add<IResourceProvider, ResourceProvider>(pluginConfiguration.ServiceLifetime);
            services.Add<ISequenceProvider, SequenceProvider>(pluginConfiguration.ServiceLifetime);

            // Add Framework
            services.Add<IWarmupTaskExecutor, WarmupTaskExecutor>(pluginConfiguration.ServiceLifetime);

            // Add Utilities
            services.Add<IAssemblyScanner, AssemblyScanner>(pluginConfiguration.ServiceLifetime);
            services.Add<IInlineValidator, InlineValidator>(pluginConfiguration.ServiceLifetime);

            // Add MemoryCache
            services.AddMemoryCache();

            // Add Configuration
            services.AddSingleton(pluginConfiguration);

            // Add UsernameProvider
            services.AddUsernameProvider(pluginConfiguration);

            return services;
        }

        public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, ServiceLifetime serviceLifetime) where TService : class where TImplementation : class, TService
        {
            var service = new ServiceDescriptor
            (
                typeof(TService),
                typeof(TImplementation),
                serviceLifetime
            );

            services.Add(service);

            return services;
        }

        public static IServiceCollection Add(this IServiceCollection services, Type type, ServiceLifetime serviceLifetime)
        {
            var service = new ServiceDescriptor
            (
                type,
                type,
                serviceLifetime
            );

            services.Add(service);

            return services;
        }

        public static IServiceCollection AddTypesWithAttribute<TAttribute>(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<TAttribute, bool> predicate = null) where TAttribute : Attribute
        {
            IEnumerable<Type> types = AssemblyScanner.Instance.FindTypesWithAttribute(assemblies, predicate);

            return services.AddTypes(types, serviceLifetime);
        }

        public static IServiceCollection AddTypesWithAttribute(this IServiceCollection services, Type type, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<object, bool> predicate = null)
        {
            IEnumerable<Type> types = AssemblyScanner.Instance.FindTypesWithAttribute(type, assemblies, predicate);

            return services.AddTypes(types, serviceLifetime);
        }

        public static IServiceCollection AddTypesWithBaseClass<TBaseClass>(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<Type, bool> predicate = null)
        {
            return services.AddTypesWithBaseClass(typeof(TBaseClass), assemblies, serviceLifetime, predicate);
        }

        public static IServiceCollection AddTypesWithBaseClass(this IServiceCollection services, Type type, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<Type, bool> predicate = null)
        {
            IEnumerable<Type> types = AssemblyScanner.Instance.FindTypesWithBaseClass(type, assemblies, predicate);

            return services.AddTypes(types, serviceLifetime);
        }

        public static IServiceCollection AddTypesWithInterface<TInterface>(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<Type, bool> predicate = null)
        {
            return services.AddTypesWithInterface(typeof(TInterface), assemblies, serviceLifetime, predicate);
        }

        public static IServiceCollection AddTypesWithInterface(this IServiceCollection services, Type type, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Func<Type, bool> predicate = null)
        {
            IEnumerable<Type> types = AssemblyScanner.Instance.FindTypesWithInterface(type, assemblies, predicate);

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

        public static IServiceCollection AddSingletonUsernameProvider(this IServiceCollection services, string username)
        {
            var usernameProvider = new UsernameProvider();

            usernameProvider.Set(username);

            services.AddSingleton<IUsernameProvider>(usernameProvider);

            return services;
        }

        public static IEnumerable<Assembly> AsEnumerable(this Assembly assembly)
        {
            return new List<Assembly> { assembly };
        }

        private static void AddUsernameProvider(this IServiceCollection services, PluginConfiguration pluginConfiguration)
        {
            if (!string.IsNullOrEmpty(pluginConfiguration.GlobalUsername))
            {
                services.AddSingletonUsernameProvider(pluginConfiguration.GlobalUsername);
            }
            else
            {
                services.Add<IUsernameProvider, UsernameProvider>(pluginConfiguration.ServiceLifetime);
            }
        }
    }
}

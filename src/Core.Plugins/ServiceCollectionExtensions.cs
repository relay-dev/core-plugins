using Core.Application;
using Core.Caching;
using Core.Ftp;
using Core.Plugins.Application;
using Core.Plugins.Caching;
using Core.Plugins.Framework;
using Core.Plugins.Ftp;
using Core.Plugins.Providers;
using Core.Plugins.Security;
using Core.Plugins.Smtp;
using Core.Plugins.Utilities;
using Core.Plugins.Validation;
using Core.Providers;
using Core.Security;
using Core.Smtp;
using Core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultCorePlugins(this IServiceCollection services)
        {
            // Add Application services
            services.AddScoped<IConnectionStringParser, ConnectionStringParser>();

            // Add Cache
            services.AddScoped<ICache, DistributedCache>();

            // Add Ftp
            services.AddScoped<IFtpClient, SystemFtpClient>();

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

            // Add Security
            services.AddScoped<ICryptographyService, AesCryptographyService>();
            services.AddScoped<IHashingService, Sha256HashingService>();

            // Add Smtp
            services.AddScoped<ISmtpClient, SystemSmtpClient>();
            services.AddScoped<ISmtpClientFactory, SmtpClientFactory>();

            // Add Utilities
            services.AddScoped<IAssemblyScanner, AssemblyScanner>();
            services.AddScoped<IGlobalHelper, GlobalHelperWrapper>();
            services.AddScoped<IInlineValidator, InlineValidator>();
            services.AddScoped<IJsonSerializer, SystemJsonSerializer>();

            return services;
        }

        public static IServiceCollection AddWarmup(this IServiceCollection services, List<Type> warmupTypes)
        {
            if (warmupTypes == null || !warmupTypes.Any())
            {
                return services;
            }

            // Add Warmup types
            warmupTypes.ForEach(warmupType =>
            {
                services.AddTransient(warmupType);
            });

            return services;
        }
    }
}

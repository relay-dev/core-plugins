using Core.Plugins.Configuration;
using Core.Plugins.EntityFramework.Auditor;
using Core.Plugins.EntityFramework.Auditor.Impl;
using Core.Plugins.EntityFramework.Providers;
using Core.Plugins.EntityFramework.Providers.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Plugins.EntityFramework
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkPlugin(this IServiceCollection services, PluginConfiguration pluginConfiguration)
        {
            services.Add<IEntityAuditor, EntityFrameworkEntityAuditor>(pluginConfiguration.ServiceLifetime);
            services.Add<IDbContextProvider, DbContextProvider>(pluginConfiguration.ServiceLifetime);

            return services;
        }
    }
}

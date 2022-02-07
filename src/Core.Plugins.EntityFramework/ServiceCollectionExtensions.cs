using Core.Plugins.Configuration;
using Core.Plugins.EntityFramework.Auditor;
using Core.Plugins.EntityFramework.Auditor.Impl;
using Core.Plugins.EntityFramework.Providers;
using Core.Plugins.EntityFramework.Providers.Impl;
using Microsoft.EntityFrameworkCore;
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

        public static IServiceCollection AddEntityFrameworkPlugin<TDbContext>(this IServiceCollection services, PluginConfiguration pluginConfiguration) where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>();

            return services.AddEntityFrameworkPlugin(pluginConfiguration);
        }
    }
}

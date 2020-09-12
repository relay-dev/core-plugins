using Core.Plugins.EntityFramework.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Plugins.EntityFramework
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkPlugin(this IServiceCollection services)
        {
            services.AddScoped<IEntityAuditor, EntityFrameworkEntityAuditor>();

            return services;
        }
    }
}

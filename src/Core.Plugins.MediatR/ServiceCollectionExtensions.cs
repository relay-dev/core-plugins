using Core.Plugins.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Core.Plugins.MediatR
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatRPlugin(this IServiceCollection services, PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration.CommandHandlerTypes == null || !pluginConfiguration.CommandHandlerTypes.Any())
            {
                return services;
            }

            // Add AddMediatR
            services.AddMediatR(pluginConfiguration.CommandHandlerTypes.ToArray());

            return services;
        }
    }
}

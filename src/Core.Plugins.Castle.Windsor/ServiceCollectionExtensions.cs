using Core.IoC;
using Core.Plugins.Castle.Windsor.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Plugins.Castle.Windsor
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCastleWindsorPlugin(this IServiceCollection services)
        {
            services.AddScoped<IIoCContainer, WindsorIoCContainer>();

            return services;
        }
    }
}

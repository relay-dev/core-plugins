using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.MediatR
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatRPlugin(this IServiceCollection services, List<Type> commandHandlerTypes)
        {
            if (commandHandlerTypes == null || !commandHandlerTypes.Any())
            {
                return services;
            }

            // Add AddMediatR
            services.AddMediatR(commandHandlerTypes.ToArray());

            return services;
        }
    }
}

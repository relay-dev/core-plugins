using Core.Application;
using Core.Events;
using Core.Plugins.Azure.BlobStorage;
using Core.Plugins.Azure.BlobStorage.Impl;
using Core.Plugins.Azure.EventGrid;
using Core.Plugins.Configuration;
using Core.Providers;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Plugins.Azure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureBlobStoragePlugin(this IServiceCollection services, PluginConfiguration pluginConfiguration, string connectionStringName = "DefaultStorageConnection")
        {
            if (pluginConfiguration.Configuration.GetConnectionString(connectionStringName) == null)
            {
                return services;
            }

            // Add StorageAccount
            services.AddScoped<IStorageAccount>(sp => new AzureStorageAccount(sp.GetService<IConnectionStringProvider>().Get(connectionStringName)));
            services.AddScoped<IStorageAccountFactory, AzureStorageAccountFactory>();

            return services;
        }

        public static IServiceCollection AddAzureEventGridPlugin(this IServiceCollection services, PluginConfiguration pluginConfiguration, string connectionStringName = "DefaultEventGridConnection")
        {
            if (pluginConfiguration.Configuration.GetConnectionString(connectionStringName) == null)
            {
                return services;
            }

            // Add EventGrid
            services.AddScoped<IEventGridClient>(sp =>
            {
                var connectionStringParser = sp.GetRequiredService<IConnectionStringParser>();
                var connectionStringProvider = sp.GetRequiredService<IConnectionStringProvider>();
                var connectionStringSegments = connectionStringParser.Parse(connectionStringProvider.Get(connectionStringName));

                return new EventGridClient(new TopicCredentials(connectionStringSegments["Key"]));
            });

            // Add EventClient
            services.AddScoped<IEventClient, EventGridEventClient>();

            return services;
        }
    }
}

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
            services.Add<IStorageAccount>(sp => new AzureStorageAccount(sp.GetService<IConnectionStringProvider>().Get(connectionStringName)), pluginConfiguration.ServiceLifetime);
            services.Add<IStorageAccountFactory, AzureStorageAccountFactory>(pluginConfiguration.ServiceLifetime);

            return services;
        }

        public static IServiceCollection AddAzureEventGridPlugin(this IServiceCollection services, PluginConfiguration pluginConfiguration, string connectionStringName = "DefaultEventGridConnection")
        {
            if (pluginConfiguration.Configuration.GetConnectionString(connectionStringName) == null)
            {
                return services;
            }

            // Add EventGrid
            services.Add<IEventGridClient>(sp =>
            {
                var connectionStringParser = sp.GetRequiredService<IConnectionStringParser>();
                var connectionStringProvider = sp.GetRequiredService<IConnectionStringProvider>();
                var connectionStringSegments = connectionStringParser.Parse(connectionStringProvider.Get(connectionStringName));

                return new EventGridClient(new TopicCredentials(connectionStringSegments["Key"]));
            }, pluginConfiguration.ServiceLifetime);

            // Add EventClient
            services.Add<IEventClient, EventGridEventClient>(pluginConfiguration.ServiceLifetime);

            return services;
        }
    }
}

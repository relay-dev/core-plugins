using Core.Application;
using Core.Events;
using Core.Exceptions;
using Core.Plugins.Azure.BlobStorage;
using Core.Plugins.Azure.BlobStorage.Impl;
using Core.Plugins.Azure.EventGrid;
using Core.Providers;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Plugins.Azure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureBlobStoragePlugin(this IServiceCollection services, IConfiguration config, string connectionStringName = "DefaultStorageConnection")
        {
            if (config.GetConnectionString(connectionStringName) == null)
            {
                throw new CoreException(ErrorCode.INVA, $"Could not find StorageAccount connection string with name '{connectionStringName}'");
            }

            // Add StorageAccount
            services.AddScoped<IStorageAccount>(sp => new AzureStorageAccount(sp.GetService<IConnectionStringProvider>().Get(connectionStringName)));
            services.AddScoped<IStorageAccountFactory, AzureStorageAccountFactory>();

            return services;
        }

        public static IServiceCollection AddAzureEventGridPlugin(this IServiceCollection services, IConfiguration config, string connectionStringName = "DefaultEventGridConnection")
        {
            if (config.GetConnectionString(connectionStringName) == null)
            {
                throw new CoreException(ErrorCode.INVA, $"Could not find EventGrid connection string with name '{connectionStringName}'");
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

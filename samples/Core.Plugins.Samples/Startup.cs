using ConsoleApp;
using Core.Plugins.AutoMapper;
using Core.Plugins.Azure;
using Core.Plugins.Configuration;
using Core.Plugins.EntityFramework;
using Core.Plugins.MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Plugins.Samples
{
    public class Startup : IConsoleAppStartup
    {
        private readonly PluginConfiguration _pluginConfiguration;

        public Startup(IConfiguration configuration)
        {
            _pluginConfiguration = BuildPluginConfiguration(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add Core Plugins
            services.AddApplicationServices(_pluginConfiguration);
            services.AddCorePlugins(_pluginConfiguration);
            services.AddAutoMapperPlugin(_pluginConfiguration);
            services.AddAzureBlobStoragePlugin(_pluginConfiguration);
            services.AddAzureEventGridPlugin(_pluginConfiguration);
            services.AddEntityFrameworkPlugin();
            services.AddMediatRPlugin(_pluginConfiguration);
            services.AddWarmup(_pluginConfiguration);
        }

        private PluginConfiguration BuildPluginConfiguration(IConfiguration configuration)
        {
            return new PluginConfigurationBuilder()
                .UseConfiguration(configuration)
                .UseApplicationName(GetType().Assembly.FullName)
                .UseServiceLifetime(ServiceLifetime.Transient)
                //.UseMappersFromAssemblyContaining<AutoMappers>()
                .UseGlobalUsername(configuration["GlobalUsername"])
                .Build();
        }
    }
}

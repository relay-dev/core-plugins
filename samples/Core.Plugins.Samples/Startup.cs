using ConsoleApp;
using Core.Plugins.AutoMapper;
using Core.Plugins.Azure;
using Core.Plugins.Configuration;
using Core.Plugins.EntityFramework;
using Core.Plugins.MediatR;
using Core.Plugins.Samples.Domain.Commands.Create;
using Core.Plugins.Samples.Domain.Context;
using Core.Plugins.Samples.Domain.Mappers;
using Core.Plugins.Samples.Domain.Validators;
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
            services.AddDbContext<OrderContext>();

            services.AddApplicationServices(_pluginConfiguration);
            services.AddCorePlugins(_pluginConfiguration);
            services.AddAutoMapperPlugin(_pluginConfiguration);
            services.AddAzureBlobStoragePlugin(_pluginConfiguration);
            services.AddAzureEventGridPlugin(_pluginConfiguration);
            services.AddEntityFrameworkPlugin(_pluginConfiguration);
            services.AddMediatRPlugin(_pluginConfiguration);
            services.AddWarmup(_pluginConfiguration);
        }

        private PluginConfiguration BuildPluginConfiguration(IConfiguration configuration)
        {
            return new PluginConfigurationBuilder()
                .UseConfiguration(configuration)
                .UseApplicationName("Samples")
                .UseServiceLifetime(ServiceLifetime.Transient)
                .UseGlobalUsername(configuration["GlobalUsername"])
                .UseCommandHandlers(options => options.FromAssemblyContaining<CreateOrderHandler>())
                .UseMappers(options => options.FromAssemblyContaining<AutoMappers>())
                .UseValidatorsFromAssemblyContaining<OrderValidator>()
                .Build();
        }
    }
}

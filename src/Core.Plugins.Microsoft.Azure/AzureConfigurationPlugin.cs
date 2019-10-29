using Core.IoC.Plugins;
using Core.Plugins.Microsoft.Azure.Wrappers;
using Core.Providers;
using static Core.Plugins.Constants.Infrastructure;

namespace Core.Plugins.Microsoft.Azure
{
    [IoCContainerPlugin(Type = PluginType.Configuration, Name = Plugin.AzureConfiguration)]
    public class AzureConfigurationPlugin : IIoCContainerPlugin
    {
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder()
                .OnInstall(iocContainer => iocContainer.Register<IConnectionStringProvider, AzureConnectionStringByConfigurationProvider>());
        }
    }
}

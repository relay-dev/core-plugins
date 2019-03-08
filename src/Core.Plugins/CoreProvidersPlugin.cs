using Core.IoC.Plugins;
using Core.Plugins.Providers;
using Core.Providers;
using static Core.Plugins.Constants.Infrastructure;

namespace Core.Plugins
{
    [IoCContainerPlugin(Type = PluginType.Providers, Name = Plugin.CoreProviders)]
    public class CoreProvidersPlugin : IIoCContainerPlugin
    {
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder()
                .OnInstall(iocContainer =>
                {
                    iocContainer.Register<IDateTimeProvider, DateTimeUtcProvider>();
                    iocContainer.Register<IRandomGuidProvider, RandomGuidProvider>();
                    iocContainer.Register<IRandomLongProvider, RandomLongProvider>();
                    iocContainer.Register<IResourceProvider, ResourceProvider>();
                    iocContainer.Register<IUsernameProvider, UsernameProvider>();
                });
        }
    }
}

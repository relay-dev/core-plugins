using Core.Caching;
using Core.IoC.Plugins;
using Core.Plugins.Microsoft.Caching.Wrappers;
using static Core.Plugins.Constants.Infrastructure;

namespace Core.Plugins.Microsoft.Caching
{
    [IoCContainerPlugin(Type = PluginType.Caching, Name = Plugin.MemoryCache)]
    public class MicrosoftCachingPlugin : IIoCContainerPlugin
    {
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder()
                .OnInstall(iocContainer => iocContainer.Register<ICache, MemoryCacheWrapper>());
        }
    }
}

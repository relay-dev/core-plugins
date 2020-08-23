using Core.Caching;
using Core.IoC.Plugins;
using Core.Plugins.Microsoft.Helpers;
using static Core.Plugins.Constants.PluginConstants.Infrastructure;

namespace Core.Plugins.Microsoft
{
    [IoCContainerPlugin(Type = PluginType.Caching, Name = Plugin.MemoryCache)]
    public class MicrosoftPlugin : IIoCContainerPlugin
    {
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder()
                .OnInstall(iocContainer => iocContainer.Register<ICacheHelper, MemoryCacheHelper>());
        }
    }
}

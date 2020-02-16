﻿using Core.Caching;
using Core.IoC.Plugins;
using Core.Plugins.Microsoft.Wrappers;
using static Core.Plugins.Constants.Infrastructure;

namespace Core.Plugins.Microsoft
{
    [IoCContainerPlugin(Type = PluginType.Caching, Name = Plugin.MemoryCache)]
    public class MicrosoftPlugin : IIoCContainerPlugin
    {
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder()
                .OnInstall(iocContainer => iocContainer.Register<ICache, MemoryCacheWrapper>());
        }
    }
}
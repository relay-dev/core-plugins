using Core.Framework;
using Core.Providers;
using System;
using static Core.Plugins.Constants.PluginConstants.Infrastructure;

namespace Core.Plugins.Providers
{
    [Component(Type = ComponentType.DateTimeProvider, Name = Component.DateTimeUtcProvider, PluginName = Plugin.CoreProviders)]
    internal class DateTimeUtcProvider : IDateTimeProvider
    {
        public DateTime Get() => DateTime.UtcNow;
    }
}

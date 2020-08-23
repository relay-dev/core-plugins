using Core.Framework.Attributes;
using Core.Providers;
using System;
using static Core.Plugins.Constants.PluginConstants.Infrastructure;

namespace Core.Plugins.Providers
{
    [Component(Type = ComponentType.DateTimeProvider, Name = Component.DateTimeProvider, PluginName = Plugin.CoreProviders)]
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Get() => DateTime.Now;
    }
}

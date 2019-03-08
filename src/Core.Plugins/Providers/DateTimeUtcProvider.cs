using Core.Framework.Attributes;
using Core.Providers;
using System;

namespace Core.Plugins.Providers
{
    [Component(Type = Constants.Infrastructure.ComponentType.DateTimeProvider, Name = Constants.Infrastructure.Component.DateTimeUtcProvider, PluginName = Constants.Infrastructure.Plugin.CoreProviders)]
    internal class DateTimeUtcProvider : IDateTimeProvider
    {
        public DateTime Get() => DateTime.UtcNow;
    }
}

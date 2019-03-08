using Core.Framework.Attributes;
using Core.Providers;
using System;

namespace Core.Plugins.Providers
{
    [Component(Type = Constants.Infrastructure.ComponentType.DateTimeProvider, Name = Constants.Infrastructure.Component.DateTimeProvider, PluginName = Constants.Infrastructure.Plugin.CoreProviders)]
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Get() => DateTime.Now;
    }
}

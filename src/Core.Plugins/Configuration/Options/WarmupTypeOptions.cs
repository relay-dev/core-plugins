using System;
using System.Collections.Generic;

namespace Core.Plugins.Configuration.Options
{
    public class WarmupTypeOptions
    {
        private readonly PluginConfiguration _pluginConfiguration;

        public WarmupTypeOptions(PluginConfiguration pluginConfiguration)
        {
            _pluginConfiguration = pluginConfiguration;
        }

        public WarmupTypeOptions FromAssemblyContaining<TValidator>()
        {
            _pluginConfiguration.WarmupAssemblies.Add(typeof(TValidator).Assembly);

            return this;
        }

        public WarmupTypeOptions FromAssemblyContaining(Type type)
        {
            _pluginConfiguration.WarmupAssemblies.Add(type.Assembly);

            return this;
        }

        public WarmupTypeOptions FromCollection(List<Type> warmupTypes)
        {
            _pluginConfiguration.WarmupTypes = warmupTypes;

            return this;
        }
    }
}

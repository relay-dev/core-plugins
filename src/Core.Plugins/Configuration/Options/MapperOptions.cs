using System;
using System.Collections.Generic;

namespace Core.Plugins.Configuration.Options
{
    public class MapperOptions
    {
        private readonly PluginConfiguration _pluginConfiguration;

        public MapperOptions(PluginConfiguration pluginConfiguration)
        {
            _pluginConfiguration = pluginConfiguration;
        }

        public MapperOptions FromAssemblyContaining<TMapper>()
        {
            _pluginConfiguration.MapperAssemblies.Add(typeof(TMapper).Assembly);

            return this;
        }

        public MapperOptions FromAssemblyContaining(Type type)
        {
            _pluginConfiguration.MapperAssemblies.Add(type.Assembly);

            return this;
        }

        public MapperOptions FromCollection(List<Type> mapperTypes)
        {
            _pluginConfiguration.MapperTypes = mapperTypes;

            return this;
        }
    }
}

using System;

namespace Core.Plugins.Configuration.Options
{
    public class MapperOptions
    {
        private readonly PluginConfiguration _configurationBuilder;

        public MapperOptions(PluginConfiguration configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        public MapperOptions FromAssemblyContaining<TCommandHandler>()
        {
            _configurationBuilder.MapperAssemblies.Add(typeof(TCommandHandler).Assembly);

            return this;
        }

        public MapperOptions FromAssemblyContaining(Type type)
        {
            _configurationBuilder.MapperAssemblies.Add(type.Assembly);

            return this;
        }
    }
}

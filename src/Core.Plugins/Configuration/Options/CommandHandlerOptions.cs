using System;
using System.Collections.Generic;

namespace Core.Plugins.Configuration.Options
{
    public class CommandHandlerOptions
    {
        private readonly PluginConfiguration _pluginConfiguration;

        public CommandHandlerOptions(PluginConfiguration configurationBuilder)
        {
            _pluginConfiguration = configurationBuilder;
        }

        public CommandHandlerOptions FromAssemblyContaining<TCommandHandler>()
        {
            _pluginConfiguration.CommandHandlerAssemblies.Add(typeof(TCommandHandler).Assembly);

            return this;
        }

        public CommandHandlerOptions FromAssemblyContaining(Type type)
        {
            _pluginConfiguration.CommandHandlerAssemblies.Add(type.Assembly);

            return this;
        }

        public CommandHandlerOptions FromCollection(List<Type> commandHandlerTypes)
        {
            _pluginConfiguration.CommandHandlerTypes = commandHandlerTypes;

            return this;
        }
    }
}

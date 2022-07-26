using System;

namespace Core.Plugins.Configuration.Options
{
    public class CommandHandlerOptions
    {
        private readonly PluginConfiguration _configurationBuilder;

        public CommandHandlerOptions(PluginConfiguration configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        public CommandHandlerOptions FromAssemblyContaining<TCommandHandler>()
        {
            _configurationBuilder.CommandHandlerAssemblies.Add(typeof(TCommandHandler).Assembly);

            return this;
        }

        public CommandHandlerOptions FromAssemblyContaining(Type type)
        {
            _configurationBuilder.CommandHandlerAssemblies.Add(type.Assembly);

            return this;
        }
    }
}

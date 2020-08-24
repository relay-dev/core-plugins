using Core.Framework;
using Core.Providers;

namespace Core.Plugins.Providers
{
    public class CommandContextProvider : ICommandContextProvider
    {
        private CommandContext _commandContext;

        public CommandContextProvider()
        {
            _commandContext = new CommandContext();
        }

        public CommandContext Get()
        {
            return _commandContext;
        }

        public void Set(CommandContext context)
        {
            _commandContext = context;
        }
    }
}

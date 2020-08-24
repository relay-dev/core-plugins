using Core.Framework;
using Core.Providers;

namespace Core.Plugins.Providers
{
    [Component]
    [Injectable]
    public class UsernameProvider : IUsernameProvider
    {
        private readonly ICommandContextProvider _commandContextProvider;

        public UsernameProvider(ICommandContextProvider commandContextProvider)
        {
            _commandContextProvider = commandContextProvider;
        }

        public string Get()
        {
            return _commandContextProvider.Get().Username;
        }
    }
}

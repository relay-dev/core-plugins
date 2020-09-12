using Core.Providers;

namespace Core.Plugins.Providers
{
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

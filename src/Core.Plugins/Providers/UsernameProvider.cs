using Core.Framework;
using Core.Providers;

namespace Core.Plugins.Providers
{
    [Component]
    [Injectable]
    internal class UsernameProvider : IUsernameProvider
    {
        private readonly string _username;

        public UsernameProvider(string username)
        {
            _username = username;
        }

        public string Get()
        {
            return _username;
        }
    }
}

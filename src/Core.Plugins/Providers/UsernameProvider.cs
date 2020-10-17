using Core.Providers;

namespace Core.Plugins.Providers
{
    public class UsernameProvider : IUsernameProvider
    {
        private string _username;

        public string Get()
        {
            return _username;
        }

        public void Set(string username)
        {
            _username = username;
        }
    }
}

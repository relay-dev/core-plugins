using Core.Providers;
using Microsoft.Extensions.Configuration;

namespace Core.Plugins.Providers
{
    public class EncryptionKeyProvider : IKeyProvider
    {
        private readonly IConfiguration _configuration;

        public EncryptionKeyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Get()
        {
            return Get("DefaultEncryptionKey");
        }

        public string Get(string keyIdentifier)
        {
            return _configuration[keyIdentifier];
        }
    }
}

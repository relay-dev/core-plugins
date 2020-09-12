using Microsoft.Extensions.Configuration;

namespace Core.Plugins.Providers
{
    public class ConnectionStringByConfigurationProvider : ConnectionStringProviderBase
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringByConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override string GetConnectionString(string connectionName)
        {
            return _configuration.GetConnectionString(connectionName);
        }

        protected override string GetPlaceholderValue(string value)
        {
            return _configuration[value];
        }
    }
}

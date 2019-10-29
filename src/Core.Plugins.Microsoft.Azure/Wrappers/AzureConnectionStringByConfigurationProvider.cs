using Microsoft.Extensions.Configuration;

namespace Core.Plugins.Microsoft.Azure.Wrappers
{
    public class AzureConnectionStringByConfigurationProvider : AzureConnectionStringProviderBase
    {
        private readonly IConfiguration _configuration;

        public AzureConnectionStringByConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override string GetConnectionString(string connectionName)
        {
            return _configuration.GetConnectionString(connectionName);
        }
    }
}

using Microsoft.Extensions.Configuration;

namespace Core.Plugins.Microsoft.Azure.Wrappers
{
    public class IntegrationTestConnectionStringProvider : AzureConnectionStringByConfigurationProvider
    {
        public IntegrationTestConnectionStringProvider(IConfiguration configuration)
            : base(configuration) { }
    }
}

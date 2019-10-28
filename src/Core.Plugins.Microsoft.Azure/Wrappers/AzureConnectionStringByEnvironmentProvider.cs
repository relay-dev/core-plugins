using System;

namespace Core.Plugins.Microsoft.Azure.Wrappers
{
    public class AzureConnectionStringByEnvironmentProvider : AzureConnectionStringProviderBase
    {
        protected override string GetConnectionString(string connectionName)
        {
            return Environment.GetEnvironmentVariable(connectionName);
        }
    }
}

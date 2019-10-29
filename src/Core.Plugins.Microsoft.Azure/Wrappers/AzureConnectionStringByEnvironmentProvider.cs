using Core.Plugins.Providers;
using System;

namespace Core.Plugins.Microsoft.Azure.Wrappers
{
    public class AzureConnectionStringByEnvironmentProvider : ConnectionStringProviderBase
    {
        protected override string GetConnectionString(string connectionName)
        {
            return Environment.GetEnvironmentVariable(connectionName);
        }
    }
}

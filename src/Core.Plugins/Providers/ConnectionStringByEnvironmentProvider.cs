using System;

namespace Core.Plugins.Providers
{
    public class ConnectionStringByEnvironmentProvider : ConnectionStringProviderBase
    {
        protected override string GetConnectionString(string connectionName)
        {
            return Environment.GetEnvironmentVariable(connectionName);
        }

        protected override string GetPlaceholderValue(string value)
        {
            return Environment.GetEnvironmentVariable(value);
        }
    }
}

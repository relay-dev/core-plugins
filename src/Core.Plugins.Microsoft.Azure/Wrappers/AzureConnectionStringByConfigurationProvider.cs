using Core.Exceptions;
using Core.Plugins.Providers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Core.Plugins.Microsoft.Azure.Wrappers
{
    public class AzureConnectionStringByConfigurationProvider : ConnectionStringProviderBase
    {
        private readonly IConfiguration _configuration;

        public AzureConnectionStringByConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override string GetConnectionString(string connectionName)
        {
            string connectionString = _configuration.GetConnectionString(connectionName);

            if (connectionString == null)
                throw new CoreException($"ConnectionName '{connectionName}' not found");

            if (connectionString == string.Empty)
                throw new CoreException($"ConnectionName '{connectionName}' cannot be an empty string");

            if (connectionString.Contains("{{"))
            {
                Dictionary<string, string> connectionStringPlaceholders = ParsePlaceholders(connectionString);

                foreach (KeyValuePair<string, string> connectionStringPlaceholder in connectionStringPlaceholders)
                {
                    if (connectionString.Contains(connectionStringPlaceholder.Key))
                    {
                        connectionString = connectionString.Replace(connectionStringPlaceholder.Key, _configuration[connectionStringPlaceholder.Value]);
                    }
                }
            }

            return connectionString;
        }
    }
}

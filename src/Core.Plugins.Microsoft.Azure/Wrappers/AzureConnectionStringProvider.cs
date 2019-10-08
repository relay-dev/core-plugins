using Core.Exceptions;
using Core.Providers;
using Microsoft.Extensions.Configuration;

namespace Core.Plugins.Microsoft.Azure.Wrappers
{
    public class AzureConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public AzureConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Get(string connectionName)
        {
            string connectionString = _configuration.GetConnectionString(connectionName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new CoreException("Connection String cannot be null or empty");
            }

            return connectionString;
        }

        public string Get()
        {
            return Get("DefaultConnection");
        }
    }
}

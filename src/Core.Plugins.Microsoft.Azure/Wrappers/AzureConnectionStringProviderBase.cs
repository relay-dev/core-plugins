using Core.Exceptions;
using Core.Providers;

namespace Core.Plugins.Microsoft.Azure.Wrappers
{
    public abstract class AzureConnectionStringProviderBase : IConnectionStringProvider
    {
        public string Get(string connectionName)
        {
            string connectionString = GetConnectionString(connectionName);

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

        protected abstract string GetConnectionString(string connectionName);
    }
}

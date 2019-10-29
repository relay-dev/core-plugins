using Core.Exceptions;
using Core.Providers;

namespace Core.Plugins.Providers
{
    public class ConnectionStringProvider : ConnectionStringProviderBase
    {
        private readonly string _connectionString;

        public ConnectionStringProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override string GetConnectionString(string connectionName)
        {
            return _connectionString;
        }
    }

    public abstract class ConnectionStringProviderBase : IConnectionStringProvider
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

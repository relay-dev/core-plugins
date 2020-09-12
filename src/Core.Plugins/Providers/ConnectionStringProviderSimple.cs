namespace Core.Plugins.Providers
{
    public class ConnectionStringProviderSimple : ConnectionStringProviderBase
    {
        private readonly string _connectionString;

        public ConnectionStringProviderSimple(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override string GetConnectionString(string connectionName)
        {
            return _connectionString;
        }

        protected override string GetPlaceholderValue(string value)
        {
            return string.Empty;
        }
    }
}

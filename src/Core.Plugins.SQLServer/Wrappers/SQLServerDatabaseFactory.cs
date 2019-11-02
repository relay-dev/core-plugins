using Core.Data;
using Core.Providers;

namespace Core.Plugins.SQLServer.Wrappers
{
    public class SQLServerDatabaseFactory : IDatabaseFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SQLServerDatabaseFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IDatabase Create(string connectionName)
        {
            return new SQLServerDatabase(_connectionStringProvider.Get(connectionName));
        }
    }
}

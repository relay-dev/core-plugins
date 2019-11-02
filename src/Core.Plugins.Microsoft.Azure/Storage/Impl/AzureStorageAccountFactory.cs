using Core.Providers;

namespace Core.Plugins.Microsoft.Azure.Storage.Impl
{
    public class AzureStorageAccountFactory : IStorageAccountFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public AzureStorageAccountFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IStorageAccount Create(string connectionName)
        {
            return new AzureStorageAccount(_connectionStringProvider.Get(connectionName));
        }
    }
}

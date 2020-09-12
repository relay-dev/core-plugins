using Core.Providers;

namespace Core.Plugins.Azure.BlobStorage.Impl
{
    public class AzureStorageAccountFactory : IStorageAccountFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public AzureStorageAccountFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IStorageAccount Create(string connectionName = null)
        {
            if (connectionName == null)
                connectionName = "DefaultStorageConnection";

            return new AzureStorageAccount(_connectionStringProvider.Get(connectionName));
        }
    }
}

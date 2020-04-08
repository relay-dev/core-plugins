namespace Core.Plugins.Microsoft.Azure.Storage
{
    public interface IStorageAccountFactory
    {
        IStorageAccount Create(string connectionName = null);
    }
}

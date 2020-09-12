namespace Core.Plugins.Azure.BlobStorage
{
    public interface IStorageAccountFactory
    {
        IStorageAccount Create(string connectionName = null);
    }
}

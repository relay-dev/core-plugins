using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace Core.Plugins.Azure.BlobStorage
{
    public interface IStorageAccount
    {
        CloudBlockBlob CreateFile(string containerName, string filename);
        Task<long> GetFileSizeAsync(string containerName, string filename);
        CloudBlockBlob GetBlobReference(string containerName, string filename);
        CloudBlockBlob GetBlobReference(string blobPath);
        Task ReadToStreamAsync(string containerName, string filename, Stream stream);
        CloudBlobClient GetStorageAccountClient();
    }
}

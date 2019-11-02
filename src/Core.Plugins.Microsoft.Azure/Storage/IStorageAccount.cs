using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace Core.Plugins.Microsoft.Azure.Storage
{
    public interface IStorageAccount
    {
        Task<CloudBlockBlob> CreateFileAsync(string containerName, string filename);

        Task<long> GetFileSizeAsync(string containerName, string filename);

        CloudBlockBlob GetBlobReference(string containerName, string filename);

        Task ReadToStreamAsync(string containerName, string filename, Stream stream);
    }
}

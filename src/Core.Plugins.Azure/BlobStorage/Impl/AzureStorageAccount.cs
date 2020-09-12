using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace Core.Plugins.Azure.BlobStorage.Impl
{
    public class AzureStorageAccount : IStorageAccount
    {
        private readonly string _connectionString;

        public AzureStorageAccount(string connectionString)
        {
            _connectionString = connectionString;
        }

        public CloudBlockBlob CreateFile(string containerName, string fileName)
        {
            return GetBlobReference(containerName, fileName);
        }

        public CloudBlockBlob GetBlobReference(string blobPath)
        {
            string containerName = Path.GetDirectoryName(blobPath).Replace("\\", string.Empty);
            string fileName = Path.GetFileName(blobPath);

            return GetBlobReference(containerName, fileName);
        }

        public CloudBlockBlob GetBlobReference(string containerName, string fileName)
        {
            CloudBlobContainer cloudBlobContainer = GetStorageAccountContainer(containerName);

            return cloudBlobContainer.GetBlockBlobReference(fileName);
        }

        public async Task<long> GetFileSizeAsync(string containerName, string fileName)
        {
            CloudBlockBlob cloudBlockBlob = GetBlobReference(containerName, fileName);

            await cloudBlockBlob.FetchAttributesAsync();

            return cloudBlockBlob.Properties.Length;
        }

        public async Task ReadToStreamAsync(string containerName, string fileName, Stream stream)
        {
            CloudBlockBlob cloudBlockBlob = GetBlobReference(containerName, fileName);

            await cloudBlockBlob.DownloadToStreamAsync(stream);
        }

        public CloudBlobClient GetStorageAccountClient()
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);

            return cloudStorageAccount.CreateCloudBlobClient();
        }

        private CloudBlobContainer GetStorageAccountContainer(string containerName)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            
            return cloudBlobClient.GetContainerReference(containerName);
        }
    }
}

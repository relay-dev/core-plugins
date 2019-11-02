using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace Core.Plugins.Microsoft.Azure.Storage.Impl
{
    public class AzureStorageAccount : IStorageAccount
    {
        private readonly string _connectionString;

        public AzureStorageAccount(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<CloudBlockBlob> CreateFileAsync(string containerName, string filename)
        {
            return GetBlobReference(containerName, filename);
        }

        public CloudBlockBlob GetBlobReference(string containerName, string filename)
        {
            CloudBlobContainer cloudBlobContainer = GetStorageAccountContainer(containerName);

            return cloudBlobContainer.GetBlockBlobReference(filename);
        }

        public async Task<long> GetFileSizeAsync(string containerName, string filename)
        {
            CloudBlockBlob cloudBlockBlob = GetBlobReference(containerName, filename);

            await cloudBlockBlob.FetchAttributesAsync();

            return cloudBlockBlob.Properties.Length;
        }

        public async Task ReadToStreamAsync(string containerName, string filename, Stream stream)
        {
            CloudBlockBlob cloudBlockBlob = GetBlobReference(containerName, filename);

            await cloudBlockBlob.DownloadToStreamAsync(stream);
        }

        #region Private

        private CloudBlobContainer GetStorageAccountContainer(string containerName)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            return cloudBlobClient.GetContainerReference(containerName);
        }

        #endregion
    }
}

using Core.FileHandling;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Core.Plugins.FileHandling.Ftp
{
    public class RensiSftpClient : FtpClientBase, IFtpClient
    {
        public RensiSftpClient(string connectionString)
            : base(connectionString) { }

        public void DeleteFile(string filePath)
        {
            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();
                
            sftpClient.DeleteFile(filePath);

            sftpClient.Disconnect();
        }

        public Stream DownloadFile(string filePath)
        {
            Stream stream = null;

            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();

            sftpClient.DownloadFile(filePath, stream);

            sftpClient.Disconnect();

            return stream;
        }

        public string GetDateTimestamp(string filePath)
        {
            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();

            var lastWriteTime = sftpClient.GetAttributes(filePath).LastWriteTime.ToString(CultureInfo.InvariantCulture);

            return lastWriteTime;
        }

        public string GetFileSize(string filePath)
        {
            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();

            var length = sftpClient.GetAttributes(filePath).Size.ToString();

            return length;
        }

        public bool IsFileExists(string filePath)
        {
            using SftpClient sftpClient = CreateSftpClient();

            return IsFileExistsUsingClient(filePath, sftpClient);
        }

        public List<string> ListDirectory(string directoryPath)
        {
            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();

            var fileNames = sftpClient.ListDirectory(directoryPath)
                .Where(file => !file.IsDirectory)
                .Select(file => file.Name)
                .ToList();

            sftpClient.Disconnect();

            return fileNames;
        }

        public List<string> ListDirectoryDetails(string directoryPath)
        {
            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();

            var fileNames = sftpClient.ListDirectory(directoryPath)
                .Where(file => !file.IsDirectory)
                .Select(file => $"{file.Name}: {{ Length: {file.Length}; LastWriteTimeUtc: {file.LastWriteTimeUtc}; LastAccessTimeUtc: {file.LastAccessTimeUtc};}}")
                .ToList();

            sftpClient.Disconnect();

            return fileNames;
        }

        public void MakeDirectory(string directoryPath)
        {
            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();

            sftpClient.CreateDirectory(directoryPath);

            sftpClient.Disconnect();
        }

        public void RemoveDirectory(string directoryPath)
        {
            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();

            sftpClient.DeleteDirectory(directoryPath);

            sftpClient.Disconnect();
        }

        public void Rename(string currentFilePath, string newFilePath)
        {
            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();

            sftpClient.RenameFile(currentFilePath, newFilePath);

            sftpClient.Disconnect();
        }

        public void UploadFile(Stream stream, string filePath)
        {
            stream.Position = 0;

            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();

            string uniqueFilePath = GetUniqueSftpFilePath(filePath, sftpClient);

            sftpClient.UploadFile(stream, uniqueFilePath);

            sftpClient.Disconnect();
        }

        public void ArchiveFile(string fileDirectory, string filename, string subDirectoryName = "", string archiveDirectoryName = "Archive", bool isUseFullPath = false)
        {
            if (!string.IsNullOrEmpty(subDirectoryName))
            {
                subDirectoryName += "/";
            }

            string currentLocation = $"{fileDirectory}/{subDirectoryName}{filename}";

            string archiveLocation = isUseFullPath
                ? string.Format("/users/{0}/{1}/{2}{3}/{4}", fileDirectory, fileDirectory, subDirectoryName, archiveDirectoryName, filename)
                : string.Format("/{0}{1}/{2}", subDirectoryName, archiveDirectoryName, filename);

            MoveFile(currentLocation, archiveLocation);
        }

        public void MoveFile(string sourcePath, string destinationPath)
        {
            using SftpClient sftpClient = CreateSftpClient();

            sftpClient.Connect();

            sftpClient.Get(sourcePath).MoveTo(destinationPath);

            sftpClient.Disconnect();
        }

        private SftpClient CreateSftpClient()
        {
            ConnectionInfo connectionInfo = new PasswordConnectionInfo(FtpClientSettings.Host, FtpClientSettings.Username, FtpClientSettings.Password);

            var client = new SftpClient(connectionInfo);

            if (FtpClientSettings.TimeoutInSeconds.HasValue)
            {
                client.OperationTimeout = TimeSpan.FromMilliseconds(FtpClientSettings.TimeoutInSeconds.Value * 1000);
            }

            return client;
        }

        private string GetUniqueSftpFilePath(string filePath, SftpClient sftpClient)
        {
            string newFileName = filePath;

            try
            {
                if (IsFileExistsUsingClient(filePath, sftpClient))
                {
                    var i = 2;

                    while (i > 0)
                    {
                        i++;

                        newFileName = Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath) + "-" + i + Path.GetExtension(filePath);

                        if (IsFileExists(newFileName))
                        {
                            continue;
                        }

                        break;
                    }
                }
            }
            catch (Exception)
            {
                return newFileName;
            }

            return newFileName;
        }

        private bool IsFileExistsUsingClient(string filePath, SftpClient sftpClient)
        {
            try
            {
                sftpClient.GetAttributes(filePath);
            }
            catch (SftpPathNotFoundException)
            {
                return false;
            }

            return true;
        }
    }
}

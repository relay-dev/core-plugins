using Core.FileHandling;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Plugins.FileHandling.FTP
{
    public class RensiSftpClient : FtpClientBase, IFtpClient
    {
        public RensiSftpClient(string connectionString)
            : base(connectionString) { }

        public void DeleteFile(string filePath)
        {
            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                sftpClient.DeleteFile(filePath);

                sftpClient.Disconnect();
            }
        }

        public Stream DownloadFile(string filePath)
        {
            Stream stream = null;

            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                sftpClient.DownloadFile(filePath, stream);

                sftpClient.Disconnect();
            }

            return stream;
        }

        public string GetDateTimestamp(string filePath)
        {
            string lastWriteTime = null;

            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                lastWriteTime = sftpClient.GetAttributes(filePath).LastWriteTime.ToString();
            }

            return lastWriteTime;
        }

        public string GetFileSize(string filePath)
        {
            string length = null;

            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                length = sftpClient.GetAttributes(filePath).Size.ToString();
            }

            return length;
        }

        public List<string> ListDirectory(string directoryPath)
        {
            var fileNames = new List<string>();

            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                fileNames = sftpClient.ListDirectory(directoryPath)
                    .Where(file => !file.IsDirectory)
                    .Select(file => file.Name)
                    .ToList();

                sftpClient.Disconnect();
            }

            return fileNames;
        }

        public List<string> ListDirectoryDetails(string directoryPath)
        {
            var fileNames = new List<string>();

            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                fileNames = sftpClient.ListDirectory(directoryPath)
                    .Where(file => !file.IsDirectory)
                    .Select(file => $"{file.Name}: {{ Length: {file.Length}; LastWriteTimeUtc: {file.LastWriteTimeUtc}; LastAccessTimeUtc: {file.LastAccessTimeUtc};}}")
                    .ToList();

                sftpClient.Disconnect();
            }

            return fileNames;
        }

        public void MakeDirectory(string directoryPath)
        {
            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                sftpClient.CreateDirectory(directoryPath);

                sftpClient.Disconnect();
            }
        }

        public void RemoveDirectory(string directoryPath)
        {
            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                sftpClient.DeleteDirectory(directoryPath);

                sftpClient.Disconnect();
            }
        }

        public void Rename(string currentFilePath, string newFilePath)
        {
            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                sftpClient.RenameFile(currentFilePath, newFilePath);

                sftpClient.Disconnect();
            }
        }

        public void UploadFile(Stream stream, string filePath)
        {
            stream.Position = 0;

            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                string uniqueFilePath = GetUniqueSftpFilePath(filePath, sftpClient);

                sftpClient.UploadFile(stream, uniqueFilePath);

                sftpClient.Disconnect();
            }
        }

        public void ArchiveFile(string fileDirectory, string filename, string subdirectoryName = "", string archiveDirectoryName = "Archive", bool isUseFullPath = false)
        {
            if (!String.IsNullOrEmpty(subdirectoryName))
                subdirectoryName = subdirectoryName + "/";

            string currentLocation = String.Format("{0}/{1}{2}", fileDirectory, subdirectoryName, filename);

            string archiveLocation = isUseFullPath
                ? String.Format("/users/{0}/{1}/{2}{3}/{4}", fileDirectory, fileDirectory, subdirectoryName, archiveDirectoryName, filename)
                : String.Format("/{0}{1}/{2}", subdirectoryName, archiveDirectoryName, filename);

            MoveFile(currentLocation, archiveLocation);
        }

        public void MoveFile(string sourcePath, string destinationPath)
        {
            using (SftpClient sftpClient = CreateSftpClient())
            {
                sftpClient.Connect();

                sftpClient.Get(sourcePath).MoveTo(destinationPath);

                sftpClient.Disconnect();
            }
        }

        public bool IsFileExists(string filePath)
        {
            using (SftpClient sftpClient = CreateSftpClient())
            {
                return IsFileExistsUsingClient(filePath, sftpClient);
            }
        }

        private SftpClient CreateSftpClient()
        {
            ConnectionInfo connectionInfo = new PasswordConnectionInfo(FtpClientSettings.Host, FtpClientSettings.Username, FtpClientSettings.Password);

            return new SftpClient(connectionInfo);
        }

        private string GetUniqueSftpFilePath(string filePath, SftpClient sftpClient)
        {
            string newfileName = filePath;

            try
            {
                if (IsFileExistsUsingClient(filePath, sftpClient))
                {
                    var i = 2;

                    while (i > 0)
                    {
                        i++;

                        newfileName = Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath) + "-" + i + Path.GetExtension(filePath);

                        if (IsFileExists(newfileName))
                        {
                            continue;
                        }

                        break;
                    }
                }
            }
            catch (Exception)
            {
                return newfileName;
            }

            return newfileName;
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

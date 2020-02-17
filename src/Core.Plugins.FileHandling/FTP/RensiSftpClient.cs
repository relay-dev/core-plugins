using Core.Files;
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

        public Stream DownloadFile(string filePath)
        {
            Stream stream = null;

            using (SftpClient sftpClient = GetSftpClient())
            {
                sftpClient.Connect();

                sftpClient.DownloadFile(filePath, stream);

                sftpClient.Disconnect();
            }

            return stream;
        }

        public List<string> ListDirectory(string directoryPath)
        {
            var fileNames = new List<string>();
            
            using (SftpClient sftpClient = GetSftpClient())
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

        public void UploadFile(FileStream fileStream, string filePath)
        {
            using (SftpClient sftpClient = GetSftpClient())
            {
                sftpClient.Connect();

                string uniqueFilePath = GetUniqueSftpFilePath(filePath, sftpClient);

                sftpClient.UploadFile(fileStream, uniqueFilePath);

                sftpClient.Disconnect();
            }
        }

        public void MoveFile(string sourcePath, string destinationPath)
        {
            using (SftpClient sftpClient = GetSftpClient())
            {
                sftpClient.Connect();

                sftpClient.Get(sourcePath).MoveTo(destinationPath);

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

        public bool IsFileExists(string filePath)
        {
            using (SftpClient sftpClient = GetSftpClient())
            {
                return IsFileExistsUsingClient(filePath, sftpClient);
            }
        }

        public void DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public string GetDateTimestamp(string filePath)
        {
            throw new NotImplementedException();
        }

        public string GetFileSize(string filePath)
        {
            throw new NotImplementedException();
        }

        public List<string> ListDirectoryDetails(string directoryPath)
        {
            throw new NotImplementedException();
        }

        public void MakeDirectory(string directoryPath)
        {
            throw new NotImplementedException();
        }

        public void RemoveDirectory(string directoryPath)
        {
            throw new NotImplementedException();
        }

        public void Rename(string currentFilePath, string newFileName)
        {
            throw new NotImplementedException();
        }

        public void UploadFile(Stream stream, string filePath)
        {
            throw new NotImplementedException();
        }

        #region Private Methods

        private SftpClient GetSftpClient()
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

        #endregion
    }
}

using Core.Files;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Core.Plugins.FileHandling.FTP
{
    public class FtpClient : FtpClientBase, IFtpClient
    {
        public FtpClient(string connectionString)
            : base(connectionString) { }

        public void DeleteFile(string filePath)
        {
            FtpWebRequest ftpWebRequest = CreateFtpRequest(filePath, WebRequestMethods.Ftp.DeleteFile);

            SendRequest(ftpWebRequest);
        }

        public Stream DownloadFile(string filePath)
        {
            FtpWebRequest ftpWebRequest = CreateFtpRequest(filePath, WebRequestMethods.Ftp.DownloadFile);

            return SendRequestAndGetResponse(ftpWebRequest);
        }

        public string GetDateTimestamp(string filePath)
        {
            FtpWebRequest ftpWebRequest = CreateFtpRequest(filePath, WebRequestMethods.Ftp.GetDateTimestamp);

            return SendRequestAndGetResponseString(ftpWebRequest);
        }

        public string GetFileSize(string filePath)
        {
            FtpWebRequest ftpWebRequest = CreateFtpRequest(filePath, WebRequestMethods.Ftp.GetFileSize);

            return SendRequestAndGetResponseString(ftpWebRequest);
        }

        public List<string> ListDirectory(string directoryPath)
        {
            FtpWebRequest ftpWebRequest = CreateFtpRequest(directoryPath, WebRequestMethods.Ftp.ListDirectory);

            return SendRequestAndGetResponseStringDelimited(ftpWebRequest);
        }

        public List<string> ListDirectoryDetails(string directoryPath)
        {
            FtpWebRequest ftpWebRequest = CreateFtpRequest(directoryPath, WebRequestMethods.Ftp.ListDirectoryDetails);

            return SendRequestAndGetResponseStringDelimited(ftpWebRequest);
        }

        public void MakeDirectory(string directoryPath)
        {
            FtpWebRequest ftpWebRequest = CreateFtpRequest(directoryPath, WebRequestMethods.Ftp.MakeDirectory);

            SendRequest(ftpWebRequest);
        }

        public void RemoveDirectory(string directoryPath)
        {
            FtpWebRequest ftpWebRequest = CreateFtpRequest(directoryPath, WebRequestMethods.Ftp.RemoveDirectory);

            SendRequest(ftpWebRequest);
        }

        public void Rename(string currentFilePath, string newFileName)
        {
            FtpWebRequest ftpWebRequest = CreateFtpRequest(currentFilePath, WebRequestMethods.Ftp.Rename);

            ftpWebRequest.RenameTo = newFileName;

            SendRequest(ftpWebRequest);
        }

        public void UploadFile(Stream stream, string filePath)
        {
            FtpWebRequest ftpWebRequest = CreateFtpRequest(filePath, WebRequestMethods.Ftp.UploadFile);

            using (Stream ftpStream = ftpWebRequest.GetRequestStream())
            {
                stream.CopyTo(ftpStream);
            }

            stream.Close();
        }

        #region Private

        private FtpWebRequest CreateFtpRequest(string filePath, string method)
        {
            FtpWebRequest ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create($"{FtpClientSettings.Host}/{filePath}");

            ftpWebRequest.Credentials = new NetworkCredential(FtpClientSettings.Username, FtpClientSettings.Password);

            ftpWebRequest.UseBinary = true;
            ftpWebRequest.UsePassive = true;
            ftpWebRequest.KeepAlive = true;

            ftpWebRequest.Method = method;

            return ftpWebRequest;
        }

        private void SendRequest(FtpWebRequest ftpWebRequest)
        {
            FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();

            ftpWebResponse.Close();
        }

        private Stream SendRequestAndGetResponse(FtpWebRequest ftpWebRequest)
        {
            FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();

            return ftpWebResponse.GetResponseStream();
        }

        private string SendRequestAndGetResponseString(FtpWebRequest ftpWebRequest)
        {
            string response;

            using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
            {
                using (Stream ftpStream = ftpWebResponse.GetResponseStream())
                {
                    using (var ftpReader = new StreamReader(ftpStream))
                    {
                        response = ftpReader.ReadToEnd();
                    }
                }
            }

            return response;
        }

        private List<string> SendRequestAndGetResponseStringDelimited(FtpWebRequest ftpWebRequest)
        {
            string response = null;

            using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
            {
                using (Stream ftpStream = ftpWebResponse.GetResponseStream())
                {
                    using (var ftpReader = new StreamReader(ftpStream))
                    {
                        while (ftpReader.Peek() != -1)
                        {
                            response += ftpReader.ReadLine() + "|";
                        }
                    }
                }
            }

            return response.Split("|".ToCharArray()).ToList();
        }

        #endregion
    }
}

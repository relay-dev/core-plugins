using Core.FileHandling;

namespace Core.Plugins.FileHandling.FTP
{
    public class FtpClientBase
    {
        private readonly string _connectionString;

        public FtpClientBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        private FtpClientSettings _ftpClientSettings;
        protected FtpClientSettings FtpClientSettings
        {
            get
            {
                return _ftpClientSettings ?? (_ftpClientSettings = new FtpClientSettings(_connectionString));
            }
        }
    }
}

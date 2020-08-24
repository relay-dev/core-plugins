using Core.Plugins.Communication.Ftp;

namespace Core.Plugins.FileHandling.Ftp
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
                return _ftpClientSettings ??= new FtpClientSettings(_connectionString);
            }
        }
    }
}

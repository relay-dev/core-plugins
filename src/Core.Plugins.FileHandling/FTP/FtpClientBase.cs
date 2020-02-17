using Core.Files;
using System.Data.SqlClient;

namespace Core.Plugins.FileHandling.FTP
{
    public class FtpClientBase
    {
        protected readonly FtpClientSettings FtpClientSettings;

        public FtpClientBase(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);

            FtpClientSettings = new FtpClientSettings
            {
                Host = builder.DataSource,
                Username = builder.UserID,
                Password = builder.Password,
                IsSftp = builder.DataSource != null && builder.DataSource.ToLower().StartsWith("sftp")
            };
        }
    }
}

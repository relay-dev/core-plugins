using Core.FileHandling;
using Core.Plugins.Communication.Ftp;
using Core.Providers;

namespace Core.Plugins.FileHandling.Ftp
{
    public class FtpClientFactory : IFtpClientFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public FtpClientFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IFtpClient Create(string connectionName = null)
        {
            if (connectionName == null)
                connectionName = "DefaultFtpConnection";

            string connectionString = _connectionStringProvider.Get(connectionName);

            var ftpClientSettings = new FtpClientSettings(connectionString);

            return ftpClientSettings.IsSftp
                ? (IFtpClient)new RensiSftpClient(connectionString)
                : (IFtpClient)new SystemFtpClient(connectionString);
        }
    }
}

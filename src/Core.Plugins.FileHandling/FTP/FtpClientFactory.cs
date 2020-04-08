using Core.FileHandling;
using Core.Providers;

namespace Core.Plugins.FileHandling.FTP
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

            var ftpClientSettings = new FtpClientSettings(connectionName);

            return ftpClientSettings.IsSftp
                ? (IFtpClient)new RensiSftpClient(_connectionStringProvider.Get(connectionName))
                : (IFtpClient)new SystemFtpClient(_connectionStringProvider.Get(connectionName));
        }
    }
}

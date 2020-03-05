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

        public IFtpClient Create(string connectionName)
        {
            bool isSftp = connectionName != null && connectionName.ToLower().StartsWith("sftp");

            return isSftp
                ? (IFtpClient)new RensiSftpClient(_connectionStringProvider.Get(connectionName))
                : (IFtpClient)new SystemFtpClient(_connectionStringProvider.Get(connectionName));
        }
    }
}

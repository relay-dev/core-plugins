using Core.Communication.Smtp;
using Core.Providers;

namespace Core.Plugins.Communication.Smtp
{
    public class SmtpClientFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SmtpClientFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public ISmtpClient Create(string connectionName)
        {
            return new SystemSmtpClient(_connectionStringProvider.Get(connectionName));
        }
    }
}

using Core.Providers;
using Core.Smtp;

namespace Core.Plugins.Smtp
{
    public class SmtpClientFactory : ISmtpClientFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SmtpClientFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public ISmtpClient Create(string connectionName = null)
        {
            connectionName ??= "DefaultSmtpConnection";

            return new SystemSmtpClient(_connectionStringProvider.Get(connectionName));
        }
    }
}

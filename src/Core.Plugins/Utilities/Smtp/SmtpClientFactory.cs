using Core.Providers;
using Core.Utilities.Smtp;

namespace Core.Plugins.Utilities.Smtp
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

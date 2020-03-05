using Core.Communication.Smtp;

namespace Core.Plugins.Communication.Smtp
{
    public class SmtpClientBase
    {
        private readonly string _connectionString;

        public SmtpClientBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SmtpClientSettings _smtpClientSettings;
        protected SmtpClientSettings SmtpClientSettings
        {
            get
            {
                return _smtpClientSettings ?? (_smtpClientSettings = new SmtpClientSettings(_connectionString));
            }
        }
    }
}

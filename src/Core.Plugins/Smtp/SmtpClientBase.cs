namespace Core.Plugins.Smtp
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
                return _smtpClientSettings ??= new SmtpClientSettings(_connectionString);
            }
        }
    }
}

using Core.Exceptions;
using Core.Utilities.Smtp;
using System;
using System.Net.Mail;

namespace Core.Plugins.Utilities.Smtp
{
    public class SystemSmtpClient : SmtpClientBase, ISmtpClient
    {
        public SystemSmtpClient(string connectionString)
            : base(connectionString) { }

        public void Send(MailMessage mailMessage)
        {
            if (string.IsNullOrEmpty(SmtpClientSettings.Host) || string.IsNullOrEmpty(SmtpClientSettings.Port))
            {
                throw new CoreException(ErrorCode.INVA, "Host and Port must be set to use SystemSmtpClient");
            }

            new SmtpClient(SmtpClientSettings.Host, Convert.ToInt32(SmtpClientSettings.Port)).Send(mailMessage);
        }
    }
}

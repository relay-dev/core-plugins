using Core.Communication.Smtp;
using Core.Exceptions;
using System;
using System.Net.Mail;

namespace Core.Plugins.Communication.Smtp
{
    public class SystemSmtpClient : SmtpClientBase, ISmtpClient
    {
        public SystemSmtpClient(string connectionString)
            : base(connectionString) { }

        public void Send(MailMessage mailMessage)
        {
            if (String.IsNullOrEmpty(SmtpClientSettings.Host) || String.IsNullOrEmpty(SmtpClientSettings.Port))
                throw new CoreException(Exceptions.ErrorCode.INVA, "Host and Port must be set to use SystemSmtpClient");

            new SmtpClient(SmtpClientSettings.Host, Convert.ToInt32(SmtpClientSettings.Port)).Send(mailMessage);
        }
    }
}

using Core.Communication.Smtp;
using Core.Exceptions;
using System;
using System.Net;
using System.Net.Mail;

namespace Core.Plugins.Communication.Smtp
{
    public class SendGridSmtpClient : SmtpClientBase, ISmtpClient
    {
        public SendGridSmtpClient(string connectionString)
            : base(connectionString) { }

        public void Send(MailMessage mailMessage)
        {
            if (String.IsNullOrEmpty(SmtpClientSettings.Host) || String.IsNullOrEmpty(SmtpClientSettings.Port) || String.IsNullOrEmpty(SmtpClientSettings.Username) || String.IsNullOrEmpty(SmtpClientSettings.Password))
                throw new CoreException(ErrorCode.INVA, "Host, Port, Username and Password must be set to use SendGridSmtpClient");

            var smtpClient = new SmtpClient(SmtpClientSettings.Host, Convert.ToInt32(SmtpClientSettings.Port))
            {
                Credentials = new NetworkCredential
                {
                    UserName = SmtpClientSettings.Username,
                    Password = SmtpClientSettings.Password
                }
            };

            smtpClient.Send(mailMessage);
        }
    }
}

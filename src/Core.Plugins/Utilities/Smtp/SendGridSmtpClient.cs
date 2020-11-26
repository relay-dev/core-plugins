using Core.Exceptions;
using Core.Utilities.Smtp;
using System;
using System.Net;
using System.Net.Mail;

namespace Core.Plugins.Utilities.Smtp
{
    public class SendGridSmtpClient : SmtpClientBase, ISmtpClient
    {
        public SendGridSmtpClient(string connectionString)
            : base(connectionString) { }

        public void Send(MailMessage mailMessage)
        {
            if (string.IsNullOrEmpty(SmtpClientSettings.Host) || string.IsNullOrEmpty(SmtpClientSettings.Port) || string.IsNullOrEmpty(SmtpClientSettings.Username) || String.IsNullOrEmpty(SmtpClientSettings.Password))
            {
                throw new CoreException(ErrorCode.INVA, "Host, Port, Username and Password must be set to use SendGridSmtpClient");
            }

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

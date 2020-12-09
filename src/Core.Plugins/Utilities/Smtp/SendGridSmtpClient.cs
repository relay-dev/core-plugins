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
            if (string.IsNullOrEmpty(Settings.Host) || string.IsNullOrEmpty(Settings.Port) || string.IsNullOrEmpty(Settings.Username) || String.IsNullOrEmpty(Settings.Password))
            {
                throw new CoreException(ErrorCode.INVA, "Host, Port, Username and Password must be set to use SendGridSmtpClient");
            }

            var smtpClient = new SmtpClient(Settings.Host, Convert.ToInt32(Settings.Port))
            {
                Credentials = new NetworkCredential
                {
                    UserName = Settings.Username,
                    Password = Settings.Password
                }
            };

            smtpClient.Send(mailMessage);
        }
    }
}

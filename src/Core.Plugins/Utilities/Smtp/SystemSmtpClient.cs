using Core.Exceptions;
using Core.Utilities.Smtp;
using System;
using System.Net;
using System.Net.Mail;

namespace Core.Plugins.Utilities.Smtp
{
    public class SystemSmtpClient : SmtpClientBase, ISmtpClient
    {
        public SystemSmtpClient(string connectionString)
            : base(connectionString) { }

        public void Send(MailMessage mailMessage)
        {
            if (string.IsNullOrEmpty(Settings.Host) || string.IsNullOrEmpty(Settings.Port))
            {
                throw new CoreException(ErrorCode.INVA, "Host and Port must be set to use SystemSmtpClient");
            }

            var client = new SmtpClient(Settings.Host, Convert.ToInt32(Settings.Port));

            if (!string.IsNullOrEmpty(Settings.Username) && !string.IsNullOrEmpty(Settings.Password))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Settings.Username, Settings.Password);
            }

            client.Send(mailMessage);
        }
    }
}

using Core.Plugins.Application;
using System;
using System.Collections.Generic;

namespace Core.Plugins.Ftp
{
    /// <summary>
    /// Settings used to connect to an FTP drive
    /// </summary>
    public class FtpClientSettings : ConnectionStringParser
    {
        /// <summary>
        /// Initialized the immutable object
        /// </summary>
        /// <param name="connectionString">The connection string of the FTP server</param>
        public FtpClientSettings(string connectionString)
        {
            Dictionary<string, string> segments = Parse(connectionString);

            Host = TryGetValueOrNull(segments, "Host");
            Port = TryGetValueOrNull(segments, "Port");
            Username = TryGetValueOrNull(segments, "Username");
            Password = TryGetValueOrNull(segments, "Password");
            TimeoutInSeconds = ParseTimeout(segments);
            IsSftp = Convert.ToBoolean(TryGetValueOrNull(segments, "IsSftp"));
        }

        /// <summary>
        /// Initialized the immutable object
        /// </summary>
        /// <param name="host">The host of the FTP drive</param>
        /// <param name="port">The port of the FTP drive</param>
        /// <param name="username">The username for the credentials of the FTP drive</param>
        /// <param name="password">The password for the credentials of the FTP drive</param>
        /// <param name="timeoutInSeconds">The duration of time an operation should wait before throwing a timeout exception</param>
        /// <param name="isSftp">Indicates is the FTP drive to connect to is SFTP</param>
        public FtpClientSettings(string host, string port, string username, string password, int? timeoutInSeconds = null, bool isSftp = false)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            TimeoutInSeconds = timeoutInSeconds;
            IsSftp = isSftp;
        }

        /// <summary>
        /// The host of the FTP drive
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// The port of the SMTP server
        /// </summary>
        public string Port { get; }

        /// <summary>
        /// The username for the credentials of the FTP drive
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// The password for the credentials of the FTP drive
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// The duration of time an operation should wait before throwing a timeout exception
        /// </summary>
        public int? TimeoutInSeconds { get; set; }

        /// <summary>
        /// Indicates is the FTP drive to connect to is SFTP
        /// </summary>
        public bool IsSftp { get; }

        private int? ParseTimeout(Dictionary<string, string> connectionStringProperties)
        {
            string timeoutStr = TryGetValueOrNull(connectionStringProperties, "TimeoutInSeconds");

            if (!string.IsNullOrEmpty(timeoutStr))
            {
                if (!int.TryParse(timeoutStr, out int timeout))
                {
                    throw new Exception($"Could not parse the FTP connection string as expected. TimeoutInSeconds must be an integer. Value found was '{timeoutStr}'");
                }

                return timeout;
            }

            return null;
        }
    }
}

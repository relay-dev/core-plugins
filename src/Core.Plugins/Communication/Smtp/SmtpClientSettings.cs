﻿using Core.Plugins.Application;
using Core.Plugins.Extensions;
using System.Collections.Generic;

namespace Core.Plugins.Communication.Smtp
{
    public class SmtpClientSettings : ConnectionStringParser
    {
        /// <summary>
        /// Initialized the immutable object
        /// </summary>
        /// <param name="connectionString">The connection string of the SMTP server</param>
        public SmtpClientSettings(string connectionString)
        {
            Dictionary<string, string> segments = Parse(connectionString);

            Host = segments.TryGetValueOrNull("Host");
            Port = segments.TryGetValueOrNull("Port");
            Username = segments.TryGetValueOrNull("Username");
            Password = segments.TryGetValueOrNull("Password");
        }

        /// <summary>
        /// Initialized the immutable object
        /// </summary>
        /// <param name="host">The host of the SMTP server</param>
        /// <param name="port">The port of the SMTP server</param>
        /// <param name="username">The username for the credentials of the SMTP server</param>
        /// <param name="password">The password for the credentials of the SMTP server</param>
        public SmtpClientSettings(string host, string port, string username, string password)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
        }

        /// <summary>
        /// The host of the SMTP server
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// The port of the SMTP server
        /// </summary>
        public string Port { get; }

        /// <summary>
        /// The username for the credentials of the SMTP server
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// The password for the credentials of the SMTP server
        /// </summary>
        public string Password { get; }
    }
}

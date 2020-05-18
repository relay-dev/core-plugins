using Core.Exceptions;
using Core.Plugins.Extensions;
using Core.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Plugins.Providers
{
    public abstract class ConnectionStringProviderBase : IConnectionStringProvider
    {
        public string Get(string connectionName)
        {
            string connectionString = GetConnectionString(connectionName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new CoreException("Connection String cannot be null or empty");
            }

            return connectionString;
        }

        public string Get()
        {
            return Get("DefaultConnection");
        }

        protected Dictionary<string, string> ParsePlaceholders(string connectionString)
        {
            var regex = new Regex(@"{{(.*?)}}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Dictionary<string, string> connectionStringVariables = regex.Matches(connectionString)
                .Select(match => match.ToString())
                .OrderBy(s => s)
                .ToDictionary(s => s, s => s.Remove("{{").Remove("}}"));

            return connectionStringVariables;
        }

        protected abstract string GetConnectionString(string connectionName);
    }

    public class SimpleConnectionStringProvider : ConnectionStringProviderBase
    {
        private readonly string _connectionString;

        public SimpleConnectionStringProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override string GetConnectionString(string connectionName)
        {
            return _connectionString;
        }
    }
}

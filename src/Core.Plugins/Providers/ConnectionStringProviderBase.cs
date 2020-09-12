using Core.Exceptions;
using Core.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Plugins.Providers
{
    public abstract class ConnectionStringProviderBase : IConnectionStringProvider
    {
        public string Get()
        {
            return Get("DefaultConnection");
        }

        public string Get(string connectionName)
        {
            string connectionString = GetOptional(connectionName);

            if (connectionString == null)
            {
                throw new CoreException($"ConnectionName '{connectionName}' not found");
            }

            if (connectionString == string.Empty)
            {
                throw new CoreException($"ConnectionName '{connectionName}' cannot be an empty string");
            }

            return connectionString;
        }

        public string GetOptional(string connectionName)
        {
            string connectionString = GetConnectionString(connectionName);

            if (connectionString.Contains("{{"))
            {
                Dictionary<string, string> connectionStringPlaceholders = ParsePlaceholders(connectionString);

                foreach (KeyValuePair<string, string> connectionStringPlaceholder in connectionStringPlaceholders)
                {
                    if (connectionString.Contains(connectionStringPlaceholder.Key))
                    {
                        connectionString = connectionString.Replace(connectionStringPlaceholder.Key, GetPlaceholderValue(connectionStringPlaceholder.Value));
                    }
                }
            }

            return connectionString;
        }

        protected Dictionary<string, string> ParsePlaceholders(string connectionString)
        {
            var regex = new Regex(@"{{(.*?)}}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Dictionary<string, string> connectionStringVariables = regex.Matches(connectionString)
                .Select(match => match.ToString())
                .OrderBy(s => s)
                .ToDictionary(s => s, s => s.Replace("{{", string.Empty).Replace("}}", string.Empty));

            return connectionStringVariables;
        }

        protected abstract string GetConnectionString(string connectionName);
        protected abstract string GetPlaceholderValue(string value);
    }
}

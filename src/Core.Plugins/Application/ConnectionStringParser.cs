using Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.Application
{
    public class ConnectionStringParser : IConnectionStringParser
    {
        public Dictionary<string, string> Parse(string connectionString)
        {
            return connectionString.Split(';')
                .Where(prop => !string.IsNullOrEmpty(prop))
                .Select(prop => prop.Split(new[] { '=' }, 2))
                .ToDictionary(prop => prop[0].Trim(), t => t[1].Trim(), StringComparer.InvariantCultureIgnoreCase);
        }
    }
}

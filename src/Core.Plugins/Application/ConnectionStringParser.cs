using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Core.Plugins.Application
{
    public class ConnectionStringParser
    {
        private readonly ExpandoObject _segments;

        public ConnectionStringParser(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("connectionString cannot be null");
            }

            ConnectionStringSegments = connectionString.Split(';')
                .Where(prop => !string.IsNullOrEmpty(prop))
                .Select(prop => prop.Split(new[] { '=' }, 2))
                .ToDictionary(prop => prop[0].Trim(), t => t[1].Trim(), StringComparer.InvariantCultureIgnoreCase);

            var expandoCollection = (ICollection<KeyValuePair<string, object>>)(_segments = new ExpandoObject());

            foreach (var kvp in ConnectionStringSegments)
            {
                expandoCollection.Add(new KeyValuePair<string, object>(kvp.Key, kvp.Value));
            }
        }

        public readonly Dictionary<string, string> ConnectionStringSegments;
        public dynamic Segment => _segments;

        public string TryGet(string key)
        {
            if (ConnectionStringSegments.ContainsKey(key))
            {
                return ConnectionStringSegments[key].ToString();
            }

            return null;
        }
    }
}

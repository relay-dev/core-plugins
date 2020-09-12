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
            var dictionary = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(connectionString))
            {
                return dictionary;
            }

            string[] segments = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (string segment in segments)
            {
                if (!segment.Contains("="))
                {
                    throw new Exception("Connections string has an invalid format. The following character must be present in each segment: =");
                }

                string[] segmentVales = segment.Split('=');

                if (segmentVales.Length == 2)
                {
                    dictionary.Add(segmentVales[0], segmentVales[1]);
                }
                else
                {
                    string segmentValue = string.Join('=', segmentVales.Skip(1));

                    dictionary.Add(segmentVales[0], segmentValue);
                }
            }

            return dictionary;
        }

        protected static TValue TryGetValueOrNull<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
        {
            bool isSuccessful = dictionary.TryGetValue(key, out var value);

            return isSuccessful
                ? value
                : null;
        }
    }
}

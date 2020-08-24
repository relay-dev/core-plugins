using System.Collections.Generic;
using System.Dynamic;

namespace Core.Plugins.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue TryGetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
        {
            bool isSuccessful = dictionary.TryGetValue(key, out var value);

            return isSuccessful
                ? value
                : null;
        }

        public static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            bool isSuccessful = dictionary.TryGetValue(key, out var value);

            return isSuccessful
                ? value
                : default(TValue);
        }

        public static dynamic ToDynamic<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var expandoCollection = (ICollection<KeyValuePair<string, object>>)new ExpandoObject();

            foreach (var kvp in dictionary)
            {
                expandoCollection.Add(new KeyValuePair<string, object>(kvp.Key.ToString(), kvp.Value));
            }

            return expandoCollection;
        }
    }
}

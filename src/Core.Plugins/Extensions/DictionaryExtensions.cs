using System.Collections.Generic;

namespace Core.Plugins.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue TryGetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
        {
            TValue value;

            bool isSuccessful = dictionary.TryGetValue(key, out value);

            return isSuccessful
                ? value
                : null;
        }

        public static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;

            bool isSuccessful = dictionary.TryGetValue(key, out value);

            return isSuccessful
                ? value
                : default(TValue);
        }
    }
}

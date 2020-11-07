using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace Core.Plugins.Caching
{
    public static class MemoryCacheExtensions
    {
        public static TReturn GetOrSet<TReturn>(this IMemoryCache cache, string key, Func<TReturn> valueFactory, int expirationInHours = 24)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(expirationInHours));

            return cache.GetOrSet(key, valueFactory, cacheEntryOptions);
        }

        public static TReturn GetOrSet<TReturn>(this IMemoryCache cache, string key, Func<TReturn> valueFactory, MemoryCacheEntryOptions options)
        {
            if (!cache.TryGetValue(key, out TReturn result))
            {
                result = valueFactory.Invoke();

                cache.Set(key, result, options);

                CacheKeyManager.Add(key);
            }

            return result;
        }

        public static Dictionary<string, object> GetContents(this IMemoryCache cache)
        {
            var contents = new Dictionary<string, object>();

            foreach (string key in CacheKeyManager.GetAllKeys())
            {
                object value = cache.Get(key);

                contents.Add(key, value);
            }

            return contents;
        }

        public static void Clear(this IMemoryCache cache)
        {
            foreach (string key in CacheKeyManager.GetAllKeys())
            {
                cache.Remove(key);
            }

            CacheKeyManager.Clear();
        }
    }
}

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.Caching
{
    public static class CacheKeyManager
    {
        private static readonly ConcurrentDictionary<string, string> CacheKeys = new ConcurrentDictionary<string, string>();

        public static void Add(string key)
        {
            CacheKeys.TryAdd(key, key);
        }

        public static void Remove(string key)
        {
            CacheKeys.TryRemove(key, out string s);
        }

        public static List<string> GetAllKeys()
        {
            return CacheKeys.Keys.ToList();
        }
    }
}

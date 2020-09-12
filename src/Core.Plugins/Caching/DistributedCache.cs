using Core.Caching;
using Core.Utilities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Plugins.Caching
{
    public class DistributedCache : ICache
    {
        private readonly IDistributedCache _cache;
        private readonly IJsonSerializer _jsonSerializer;

        public DistributedCache(IDistributedCache cache, IJsonSerializer jsonSerializer)
        {
            _cache = cache;
            _jsonSerializer = jsonSerializer;
        }

        public T GetOrSet<T>(string key, DistributedCacheEntryOptions options, Func<T> valueFactory)
        {
            CacheEntry<T> cacheItem = Get<T>(key);

            if (cacheItem == null || !cacheItem.IsValid)
            {
                T value = valueFactory.Invoke();

                cacheItem = new CacheEntry<T>(key, value);

                if (options == null)
                {
                    options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(DefaultExpirationInHours));
                }

                Set(key, value, options);
            }

            return cacheItem.Value;
        }

        public T GetOrSet<T>(string key, Func<T> valueFactory)
        {
            return GetOrSet(key, null, valueFactory);
        }

        public TReturn GetOrSet<TReturn>(string key, Func<TReturn> valueFactory, int expirationInHours = 24)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(expirationInHours));

            return GetOrSet(key, options, valueFactory);
        }

        public List<string> GetAllKeys()
        {
            return CacheKeyManager.GetAllKeys();
        }

        public object Remove(string key)
        {
            CacheEntry<object> cacheEntry = Get<object>(key);

            CacheKeyManager.Remove(key);

            _cache.Remove(key);

            return cacheEntry;
        }

        public void RemoveAll()
        {
            foreach (string key in CacheKeyManager.GetAllKeys())
            {
                Remove(key);
            }
        }

        public string FormatKey(params object[] args)
        {
            return string.Join(Delimiter, args);
        }

        private void Set<T>(string key, T item, DistributedCacheEntryOptions distributedCacheEntryOptions)
        {
            var cacheItem = new CacheEntry<T>(key, item);

            byte[] encodedCacheItem = ObjectToByteArray(cacheItem);

            _cache.Set(key, encodedCacheItem, distributedCacheEntryOptions);

            CacheKeyManager.Add(key);
        }

        private CacheEntry<T> Get<T>(string key)
        {
            CacheEntry<T> cacheItem = null;

            byte[] encodedCacheItem = _cache.Get(key);

            if (encodedCacheItem != null)
            {
                cacheItem = ByteArrayToObject<CacheEntry<T>>(encodedCacheItem);
            }

            return cacheItem;
        }

        private byte[] ObjectToByteArray<T>(T item)
        {
            string itemAsJson = _jsonSerializer.Serialize(item);

            return Encoding.UTF8.GetBytes(itemAsJson);
        }

        private T ByteArrayToObject<T>(byte[] bytes)
        {
            string itemAsJson = Encoding.UTF8.GetString(bytes);

            return _jsonSerializer.Deserialize<T>(itemAsJson);
        }

        private const int DefaultExpirationInHours = 24;
        private const string Delimiter = "::";
    }
}

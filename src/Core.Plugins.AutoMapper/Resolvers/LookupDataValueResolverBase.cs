using AutoMapper;
using Core.Plugins.AutoMapper.LookupData;
using Core.Plugins.Caching;
using Core.Providers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.AutoMapper.Resolvers
{
    public abstract class LookupDataValueResolverBase<TKey> : LookupDataResolverBase<LookupDataByKey<TKey>, string>
    {
        private readonly IMemoryCache _cache;

        protected LookupDataValueResolverBase(IConnectionStringProvider connectionStringProvider, IMemoryCache cache)
            : base(connectionStringProvider)
        {
            _cache = cache;
        }

        protected abstract Dictionary<TKey, string> GetDictionaryToCache(LookupDataByKey<TKey> lookupDataByKey);

        /// <summary>
        /// This is the protected AutoMapper method called within a mapper class when using opt.ResolveUsing()
        /// In this class, GetLookupValue() is called twice as a type of self-healing mechanism whereby if we can't find the value the first time, we'll clear the cache and try again
        /// This is because it's possible for a new value to be inserted into a lookup table after this cache is loaded but before the cache timeout expires
        /// If we don't find what we're looking for, we make a one-time assumption that the cache could be out of sync, so we refresh the cache and try one more time to get the expected value
        /// </summary>
        public override string Resolve(object source, object destination, LookupDataByKey<TKey> sourceMember, string destMember, ResolutionContext context)
        {
            if (sourceMember == null || EqualityComparer<TKey>.Default.Equals(sourceMember.Key, default(TKey)))
            {
                return null;
            }

            string cacheKey = GetCacheKey(sourceMember.TableName);

            string result = GetLookupValue(sourceMember, cacheKey);

            if (result == null)
            {
                _cache.Remove(cacheKey);

                result = GetLookupValue(sourceMember, cacheKey);
            }

            return result;
        }

        /// <summary>
        /// Exposing the AutoMapper trigger method allows clients to use this resolver outside of an AutoMapper as well
        /// </summary>
        public string Resolve(LookupDataByKey<TKey> lookupDataByKey)
        {
            return Resolve(null, null, lookupDataByKey, null, null);
        }

        private string GetLookupValue(LookupDataByKey<TKey> lookupDataByKey, string cacheKey)
        {
            Dictionary<TKey, string> lookupValues =
                _cache.GetOrSet(cacheKey, () => GetDictionaryToCache(lookupDataByKey), GetCacheTimeoutInHours(lookupDataByKey));

            KeyValuePair<TKey, string> keyValuePair = lookupValues
                .SingleOrDefault(kvp => Convert.ToInt64(kvp.Key) == Convert.ToInt64(lookupDataByKey.Key));

            if (keyValuePair.Equals(default(KeyValuePair<TKey, string>)))
            {
                return null;
            }

            return keyValuePair.Value;
        }
    }
}

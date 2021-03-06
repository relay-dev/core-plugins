﻿using AutoMapper;
using Core.Plugins.AutoMapper.LookupData;
using Core.Plugins.Caching;
using Core.Providers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.AutoMapper.Resolvers
{
    public abstract class LookupDataKeyResolverBase<TValue> : LookupDataResolverBase<LookupDataByValue, TValue>
    {
        private readonly IMemoryCache _cache;

        protected LookupDataKeyResolverBase(IConnectionStringProvider connectionStringProvider, IMemoryCache cache)
            : base(connectionStringProvider)
        {
            _cache = cache;
        }

        protected abstract Dictionary<TValue, string> GetDictionaryToCache(LookupDataByValue lookupDataByValue);

        /// <summary>
        /// This is the protected AutoMapper method called within a mapper class when using opt.ResolveUsing()
        /// In this class, GetLookupValue() is called twice as a type of self-healing mechanism whereby if we can't find the value the first time, we'll clear the cache and try again
        /// This is because it's possible for a new value to be inserted into a lookup table after this cache is loaded but before the cache timeout expires
        /// If we don't find what we're looking for, we make a one-time assumption that the cache could be out of sync, so we refresh the cache and try one more time to get the expected value
        /// </summary>
        public override TValue Resolve(object source, object destination, LookupDataByValue sourceMember, TValue destMember, ResolutionContext context)
        {
            if (sourceMember == null || string.IsNullOrEmpty(sourceMember.Value))
            {
                return default;
            }

            string cacheKey = GetCacheKey(sourceMember.TableName);

            TValue result = GetLookupValue(sourceMember, cacheKey);

            if (EqualityComparer<TValue>.Default.Equals(result, default))
            {
                _cache.Remove(cacheKey);

                result = GetLookupValue(sourceMember, cacheKey);
            }

            return result;
        }

        /// <summary>
        /// Exposing the AutoMapper trigger method allows clients to use this resolver outside of an AutoMapper as well
        /// </summary>
        public TValue Resolve(LookupDataByValue lookupDataByValue)
        {
            return Resolve(null, null, lookupDataByValue, default, null);
        }

        private TValue GetLookupValue(LookupDataByValue lookupDataByValue, string cacheKey)
        {
            Dictionary<TValue, string> lookupValues =
                _cache.GetOrSet(cacheKey, () => GetDictionaryToCache(lookupDataByValue), GetCacheTimeoutInHours(lookupDataByValue));

            KeyValuePair<TValue, string> keyValuePair = lookupValues
                .SingleOrDefault(kvp => string.Equals(kvp.Value, lookupDataByValue.Value, StringComparison.OrdinalIgnoreCase));

            if (keyValuePair.Equals(default(KeyValuePair<int, string>)))
            {
                return default;
            }

            return keyValuePair.Key;
        }
    }
}
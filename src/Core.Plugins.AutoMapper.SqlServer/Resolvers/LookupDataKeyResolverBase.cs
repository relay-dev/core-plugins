﻿using AutoMapper;
using Core.Caching;
using Core.Plugins.AutoMapper.SqlServer.LookupData;
using Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.AutoMapper.SqlServer.Resolvers
{
    public abstract class LookupDataKeyResolverBase<T> : LookupDataResolverBase<LookupDataByValue, T>
    {
        private readonly ICacheHelper _cacheHelper;

        protected LookupDataKeyResolverBase(IConnectionStringProvider connectionStringProvider, ICacheHelper cacheHelper)
            : base(connectionStringProvider)
        {
            _cacheHelper = cacheHelper;
        }

        protected abstract Dictionary<T, string> GetDictionaryToCache(LookupDataByValue lookupDataByValue);

        /// <summary>
        /// This is the protected AutoMapper method called within a mapper class when using opt.ResolveUsing()
        /// In this class, GetLookupValue() is called twice as a type of self-healing mechanism whereby if we can't find the value the first time, we'll clear the cache and try again
        /// This is because it's possible for a new value to be inserted into a lookup table after this cache is loaded but before the cache timeout expires
        /// If we don't find what we're looking for, we make a one-time assumption that the cache could be out of sync, so we refresh the cache and try one more time to get the expected value
        /// </summary>
        public override T Resolve(object source, object destination, LookupDataByValue sourceMember, T destMember, ResolutionContext context)
        {
            if (sourceMember == null || String.IsNullOrEmpty(sourceMember.Value))
            {
                return default(T);
            }

            string cacheKey = GetCacheKey(sourceMember.TableName);

            T result = GetLookupValue(sourceMember, cacheKey);

            if (EqualityComparer<T>.Default.Equals(result, default(T)))
            {
                _cacheHelper.Remove(cacheKey);

                result = GetLookupValue(sourceMember, cacheKey);
            }

            return result;
        }

        /// <summary>
        /// Exposing the AutoMapper trigger method allows clients to use this resolver outside of an AutoMapper as well
        /// </summary>
        public T Resolve(LookupDataByValue lookupDataByValue)
        {
            return Resolve(null, null, lookupDataByValue, default(T), null);
        }

        private T GetLookupValue(LookupDataByValue lookupDataByValue, string cacheKey)
        {
            Dictionary<T, string> lookupValues =
                _cacheHelper.GetOrSet(cacheKey, () => GetDictionaryToCache(lookupDataByValue), GetCacheTimeoutInHours(lookupDataByValue));

            KeyValuePair<T, string> keyValuePair = lookupValues
                .SingleOrDefault(kvp => String.Equals(kvp.Value, lookupDataByValue.Value, StringComparison.OrdinalIgnoreCase));

            if (keyValuePair.Equals(default(KeyValuePair<int, string>)))
            {
                return default(T);
            }

            return keyValuePair.Key;
        }
    }
}
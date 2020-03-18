using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Caching;
using Core.Plugins.AutoMapper.Data.LookupData;

namespace Core.Plugins.AutoMapper.Data.Resolvers.Base
{
    public abstract class LookupDataValueResolverBase<T> : LookupDataResolverBase<LookupDataByKey<T>, string>
    {
        private readonly ICacheHelper _cacheHelper;

        protected LookupDataValueResolverBase(ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
        }

        protected abstract Dictionary<T, string> GetDictionaryToCache(LookupDataByKey<T> lookupDataByKey);

        /// <summary>
        /// This is the protected AutoMapper method called within a mapper class when using opt.ResolveUsing()
        /// In this class, GetLookupValue() is called twice as a type of self-healing mechanism whereby if we can't find the value the first time, we'll clear the cache and try again
        /// This is because it's possible for a new value to be inserted into a lookup table after this cache is loaded but before the cache timeout expires
        /// If we don't find what we're looking for, we make a one-time assumption that the cache could be out of sync, so we refresh the cache and try one more time to get the expected value
        /// </summary>
        public override string Resolve(object source, object destination, LookupDataByKey<T> sourceMember, string destMember, ResolutionContext context)
        {
            if (sourceMember == null || EqualityComparer<T>.Default.Equals(sourceMember.Key, default(T)))
            {
                return null;
            }

            string cacheKey = GetCacheKey(sourceMember.TableName);

            string result = GetLookupValue(sourceMember, cacheKey);

            if (result == null)
            {
                _cacheHelper.Remove(cacheKey);

                result = GetLookupValue(sourceMember, cacheKey);
            }

            return result;
        }

        /// <summary>
        /// Exposing the AutoMapper trigger method allows clients to use this resolver outside of an AutoMapper as well
        /// </summary>
        public string Resolve(LookupDataByKey<T> lookupDataByKey)
        {
            return Resolve(null, null, lookupDataByKey, null, null);
        }

        #region Private

        private string GetLookupValue(LookupDataByKey<T> lookupDataByKey, string cacheKey)
        {
            Dictionary<T, string> lookupValues =
                _cacheHelper.GetOrSet(cacheKey, () => GetDictionaryToCache(lookupDataByKey), GetCacheTimeoutInHours(lookupDataByKey));

            KeyValuePair<T, string> keyValuePair = lookupValues
                .SingleOrDefault(kvp => Convert.ToInt64(kvp.Key) == Convert.ToInt64(lookupDataByKey.Key));

            if (keyValuePair.Equals(default(KeyValuePair<T, string>)))
            {
                return null;
            }

            return keyValuePair.Value;
        }

        #endregion
    }
}

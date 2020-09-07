using AutoMapper;
using Core.Caching;
using Core.Plugins.AutoMapper.LookupData;
using Core.Providers;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.AutoMapper.Converters
{
    public abstract class LookupDataKeyConverterBase<T> : LookupDataConverterBase<LookupDataByKey<T>, string>
    {
        private readonly ICacheHelper _cacheHelper;

        protected LookupDataKeyConverterBase(IConnectionStringProvider connectionStringProvider, ICacheHelper cacheHelper)
            : base(connectionStringProvider)
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
        public override string Convert(LookupDataByKey<T> source, string destination, ResolutionContext context)
        {
            if (source == null || EqualityComparer<T>.Default.Equals(source.Key, default))
            {
                return null;
            }

            string cacheKey = GetCacheKey(source.TableName);

            string result = GetLookupValue(source, cacheKey);

            if (result == null)
            {
                _cacheHelper.Remove(cacheKey);

                result = GetLookupValue(source, cacheKey);
            }

            return result;
        }

        /// <summary>
        /// Exposing the AutoMapper trigger method allows clients to use this resolver outside of an AutoMapper as well
        /// </summary>
        public string Convert(LookupDataByKey<T> lookupDataByKey)
        {
            return Convert(lookupDataByKey, null, null);
        }

        private string GetLookupValue(LookupDataByKey<T> lookupDataByKey, string cacheKey)
        {
            Dictionary<T, string> lookupValues =
                _cacheHelper.GetOrSet(cacheKey, () => GetDictionaryToCache(lookupDataByKey), GetCacheTimeoutInHours(lookupDataByKey));

            KeyValuePair<T, string> keyValuePair = lookupValues
                .SingleOrDefault(kvp => long.Parse(kvp.Key.ToString()) == long.Parse(lookupDataByKey.Key.ToString()));

            if (keyValuePair.Equals(default(KeyValuePair<T, string>)))
            {
                return null;
            }

            return keyValuePair.Value;
        }
    }
}

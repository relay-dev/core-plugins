using Core.Caching;
using Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Core.Plugins.Microsoft.Helpers
{
    public class MemoryCacheHelper : ICacheHelper
    {
        private readonly ObjectCache _objectCache;
        private readonly IConfiguration _configuration;

        public MemoryCacheHelper()
        {
            _objectCache = MemoryCache.Default;
        }

        public MemoryCacheHelper(IConfiguration configuration)
        {
            _objectCache = MemoryCache.Default;
            _configuration = configuration;
        }

        public bool ContainsKey(string key)
        {
            key = GetKeyForApplication(key);

            return _objectCache.Contains(key);
        }

        public string FormatKey(params object[] args)
        {
            return String.Join(_delimeter, args);
        }

        public TReturn GetOrSet<TReturn>(string key, Func<TReturn> valueFactory)
        {
            return GetOrSet(key, valueFactory, TimeoutInHours);
        }

        public TReturn GetOrSet<TReturn>(string key, Func<TReturn> valueFactory, int expirationInHours = 2)
        {
            TReturn value;
            key = GetKeyForApplication(key);

            value = (TReturn)_objectCache.Get(key);

            if (value == null)
            {
                lock (__lockObject)
                {
                    value = (TReturn)_objectCache.Get(key);

                    if (value == null)
                    {
                        TimeSpan cacheExpiration = TimeSpan.FromHours(expirationInHours);

                        var offset = new DateTimeOffset(DateTime.Now.Add(cacheExpiration));

                        value = valueFactory.Invoke();

                        if (value != null)
                        {
                            _objectCache.Add(key, value, offset);
                        }
                    }
                }
            }

            return value;
        }

        public object Remove(string key)
        {
            key = GetKeyForApplication(key);

            return _objectCache.Remove(key);
        }

        public void RemoveAll()
        {
            foreach (KeyValuePair<string, object> keyValuePair in _objectCache.Where(kvp => !kvp.Key.Contains(Constants.Keywords.ActiveLogins)))
            {
                _objectCache.Remove(keyValuePair.Key);
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder($"Cache contents:");

            foreach (KeyValuePair<string, object> keyValuePair in _objectCache.Where(kvp => !kvp.Key.Contains(Constants.Keywords.ActiveLogins)))
            {
                stringBuilder.AppendLine(Environment.NewLine);
                stringBuilder.AppendLine(String.Empty.PadRight(50, '='));
                stringBuilder.AppendLine($"Cache key   : {keyValuePair.Key}");
                stringBuilder.AppendLine($"Cache value : {keyValuePair.Value}");
            }

            return stringBuilder.ToString();
        }

        #region Private

        private string GetKeyForApplication(string key)
        {
            if (ApplicationName == string.Empty)
            {
                return key;
            }
            
            return $"{ApplicationName}{_delimeter}{key}";
        }

        private Func<CacheEntry<T>> GetValueFactoryBoxed<T>(string key, Func<T> valueFactory)
        {
            Func<CacheEntry<T>> valueFactoryBoxed = () =>
            {
                T val = valueFactory.Invoke();

                return new CacheEntry<T>(key, val);
            };

            return valueFactoryBoxed;
        }

        private string ApplicationName
        {
            get
            {
                if (_configuration == null)
                {
                    return string.Empty;
                }

                return _configuration.GetAppSetting<string>(Constants.Configuration.AppSettings.ApplicationName);
            }
        }

        private int TimeoutInHours
        {
            get
            {
                if (_configuration == null)
                {
                    return _defaultTimeoutInHours;
                }

                string appSetting = _configuration.GetAppSetting<string>(Constants.Configuration.AppSettings.CacheExpirationInHours);

                return string.IsNullOrEmpty(appSetting)
                    ? _defaultTimeoutInHours
                    : Convert.ToInt32(appSetting);
            }
        }

        #endregion

        #region Static

        private static readonly Object __lockObject = new Object();
        private static readonly int _defaultTimeoutInHours = 2;
        private static readonly string _delimeter = "::";

        #endregion
    }
}

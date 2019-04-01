using Core.Caching;
using Core.Configuration;
using Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Core.Plugins.Microsoft.Caching.Wrappers
{
    public class MemoryCacheWrapper : ICache
    {
        private readonly Lazy<IConfiguration> _configuration;
        private readonly Lazy<IApplicationContextProvider> _applicationContext;

        public MemoryCacheWrapper(Lazy<IConfiguration> configuration, Lazy<IApplicationContextProvider> applicationContext)
        {
            _configuration = configuration;
            _applicationContext = applicationContext;
        }

        public string Contents => this.ToString();

        public bool ContainsKey(string key)
        {
            key = GetApplicationSpecificKey(key);

            return _objectCache.Contains(key);
        }

        public string FormatKey(params object[] args)
        {
            return String.Join(_delimeter, args);
        }

        public TReturn GetOrAdd<TReturn>(string key, Func<TReturn> valueFactory)
        {
            string cacheAppSeting = _configuration.Value.GetAppSetting<string>(Constants.Configuration.AppSettings.CacheExpirationInHours);

            int cacheExpirationInHours = String.IsNullOrEmpty(cacheAppSeting)
                ? _defaultTimeoutInHours
                : Convert.ToInt32(cacheAppSeting);

            return GetOrAdd(key, valueFactory, cacheExpirationInHours);
        }

        public TReturn GetOrAdd<TReturn>(string key, Func<TReturn> valueFactory, int expirationInHours = 2)
        {
            TReturn value;
            key = GetApplicationSpecificKey(key);

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
            key = GetApplicationSpecificKey(key);

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

        private string GetApplicationSpecificKey(string key)
        {
            string applicationName = _applicationContext.Value.Get().ApplicationName ?? _configuration.Value.GetAppSetting<string>(Constants.Configuration.AppSettings.ApplicationName);

            if (String.IsNullOrEmpty(applicationName))
            {
                throw new Exception("You cannot use Caching without an ApplicationName key in the appSettings section of your config file. The cache pool is shared, so you need to specify your application to distinguish it from other applications.");
            }

            if (applicationName != null && !key.Contains($"Application: {applicationName}"))
            {
                key = $"{applicationName}{_delimeter}{key}";
            }

            return key;
        }

        private Func<CacheEntry<T>> GetValueFactoryBoxed<T>(Func<T> valueFactory)
        {
            Func<CacheEntry<T>> valueFactoryBoxed = () =>
            {
                T val = valueFactory.Invoke();

                return new CacheEntry<T>(val);
            };

            return valueFactoryBoxed;
        }

        #endregion

        #region Static

        private static readonly ObjectCache _objectCache = MemoryCache.Default;
        private static readonly Object __lockObject = new Object();
        private static readonly int _defaultTimeoutInHours = 2;
        private static readonly string _delimeter = "::";

        #endregion
    }
}

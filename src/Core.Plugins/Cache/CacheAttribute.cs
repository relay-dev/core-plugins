using System;

namespace Core.Plugins.Cache
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CacheAttribute : Attribute
    {
        private double _expirationTime = 120.0D;

        public double ExpirationTime
        {
            get { return _expirationTime; }
            set { _expirationTime = value; }
        }

        public TimeSpan GetCacheDuration()
        {
            return TimeSpan.FromMinutes(ExpirationTime);
        }
    }
}

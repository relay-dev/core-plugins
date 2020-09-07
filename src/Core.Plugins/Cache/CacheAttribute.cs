using System;

namespace Core.Plugins.Cache
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CacheAttribute : Attribute
    {
        public double ExpirationTime { get; set; } = 120.0D;

        public TimeSpan GetCacheDuration()
        {
            return TimeSpan.FromMinutes(ExpirationTime);
        }
    }
}

using Core.Providers;
using System;

namespace Core.Plugins.Providers
{
    public class DateTimeUtcProvider : IDateTimeProvider
    {
        public DateTime Get() => DateTime.UtcNow;
    }
}

using Core.Providers;
using System;

namespace Core.Plugins.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Get() => DateTime.Now;
    }
}

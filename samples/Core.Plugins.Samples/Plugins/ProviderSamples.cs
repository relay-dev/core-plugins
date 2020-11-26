using ConsoleApp;
using Core.Providers;
using System;
using System.Threading.Tasks;

namespace Core.Plugins.Samples.Plugins
{
    [ConsoleAppMenu]
    public class ProviderSamples : ConsoleAppMenu
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public ProviderSamples(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        [ConsoleAppMenuOption]
        public async Task DateTimeProvider()
        {
            DateTime dateTime = _dateTimeProvider.Get();

            Console.WriteLine(dateTime);
        }
    }
}

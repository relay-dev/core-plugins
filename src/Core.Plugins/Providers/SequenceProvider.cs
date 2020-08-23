using Core.Framework;
using Core.IoC;
using Core.Providers;
using System.Collections.Concurrent;

namespace Core.Plugins.Providers
{
    [Component]
    [Injectable(Lifetime = RegistrationLifetime.Singleton)]
    internal class SequenceProvider : ISequenceProvider
    {
        private readonly ConcurrentDictionary<string, long> _sequences;

        public SequenceProvider()
        {
            _sequences = new ConcurrentDictionary<string, long>();

            _sequences.AddOrUpdate(Global, 0, (key, value) => 0);
        }

        public long Get()
        {
            return Get(Global);
        }

        public long Get(string sequenceName)
        {
            return _sequences.AddOrUpdate(sequenceName, 0, (key, value) => value++);
        }

        private string Global => "Global";
    }
}

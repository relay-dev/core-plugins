using Core.Exceptions;
using Core.Framework;
using Core.Plugins.Extensions;
using Core.Providers;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.Providers
{
    [Component]
    [Injectable]
    public class KeyProvider : IKeyProvider
    {
        private readonly IDictionary<string, string> _keys;

        public KeyProvider(IDictionary<string, string> keys)
        {
            _keys = keys;
        }

        public string Get()
        {
            if (!_keys.Any())
            {
                throw new CoreException(ErrorCode.INVA, $"{GetType().Name} was not initialized");
            }

            return _keys.First().Value;
        }

        public string Get(string keyIdentifier)
        {
            return _keys.TryGetValueOrNull(keyIdentifier);
        }
    }
}

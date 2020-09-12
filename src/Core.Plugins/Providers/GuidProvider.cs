using Core.Providers;
using System;

namespace Core.Plugins.Providers
{
    public class GuidProvider : IGuidProvider
    {
        public Guid Get() => Guid.NewGuid();
    }
}

using Core.Framework.Attributes;
using Core.Providers;
using System;

namespace Core.Plugins.Providers
{
    [Component]
    public class RandomGuidProvider : IRandomGuidProvider
    {
        public Guid Get() => Guid.NewGuid();
    }
}

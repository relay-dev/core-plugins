using Core.Framework.Attributes;
using Core.Providers;
using System;

namespace Core.Plugins.Providers
{
    [Component]
    [Injectable]
    public class RandomGuidProvider : IGuidProvider
    {
        public Guid Get() => Guid.NewGuid();
    }
}

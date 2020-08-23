using Core.Framework;
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

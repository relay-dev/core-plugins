using Core.Framework.Attributes;
using Core.Providers;
using System.Threading;

namespace Core.Plugins.Providers
{
    [Component]
    public class CancellationTokenProvider : ICancellationTokenProvider
    {
        public CancellationToken Get()
        {
            return new CancellationToken();
        }
    }
}

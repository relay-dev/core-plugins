using Core.Framework;
using Core.Providers;
using System.Threading;

namespace Core.Plugins.Providers
{
    [Component]
    [Injectable]
    public class CancellationTokenProvider : ICancellationTokenProvider
    {
        public CancellationToken Get()
        {
            return new CancellationToken();
        }
    }
}

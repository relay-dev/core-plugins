using Core.Framework;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Plugins.Framework
{
    public abstract class WarmupTask : IWarmup
    {
        protected readonly ILogger Logger;

        protected WarmupTask(ILogger logger)
        {
            Logger = logger;
        }
        
        protected abstract Task OnWarmupAsync(CancellationToken cancellationToken);

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            await OnWarmupAsync(cancellationToken);

            stopwatch.Stop();

            Logger.LogInformation($"WarmupTask '{GetType().Name}' completed in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}

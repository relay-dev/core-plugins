using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Plugins.Framework
{
    public class WarmupTaskExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Type> _warmupTypesToExecute;

        public WarmupTaskExecutor(IServiceProvider serviceProvider, List<Type> warmupTypesToExecute)
        {
            _serviceProvider = serviceProvider;
            _warmupTypesToExecute = warmupTypesToExecute;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            foreach (Type warmupType in _warmupTypesToExecute)
            {
                var warmupTask = (WarmupTask)_serviceProvider.GetRequiredService(warmupType);

                tasks.Add(warmupTask.RunAsync(cancellationToken));
            }

            await Task.WhenAll(tasks);
        }
    }
}

using Core.Framework;
using Core.Plugins.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Plugins.Framework
{
    public class WarmupTaskExecutor : IWarmupTaskExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PluginConfiguration _pluginConfiguration;

        public WarmupTaskExecutor(IServiceProvider serviceProvider, PluginConfiguration pluginConfiguration)
        {
            _serviceProvider = serviceProvider;
            _pluginConfiguration = pluginConfiguration;
        }
        
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            foreach (Type warmupType in _pluginConfiguration.WarmupTypes)
            {
                var warmupTask = (WarmupTask)_serviceProvider.GetRequiredService(warmupType);

                tasks.Add(warmupTask.RunAsync(cancellationToken));
            }

            await Task.WhenAll(tasks);
        }
    }
}

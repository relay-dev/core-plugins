using System;
using Xunit;

namespace Core.Plugins.xUnit.Integration
{
    public class ServiceProviderFixture<TStartup> : IDisposable where TStartup : IStartup, new()
    {
        public IServiceProvider ServiceProvider;

        public ServiceProviderFixture()
        {
            ServiceProvider = new TStartup().ConfigureServices();
        }

        public void Dispose()
        {
            ServiceProvider = null;
        }
    }

    /// <summary>
    /// This is needed for xUnit
    /// </summary>
    [CollectionDefinition("Service Provider collection")]
    public class ServiceProviderCollection<TStartup> : ICollectionFixture<ServiceProviderFixture<TStartup>> where TStartup : IStartup, new()
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}

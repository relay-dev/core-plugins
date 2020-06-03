using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit.Abstractions;

namespace Core.Plugins.xUnit.Integration
{
    public abstract class IntegrationTest<TStartup, TSUT> : IntegrationTest where TStartup : IStartup, new()
    {
        // Give each test their own instance
        protected TSUT SUT => ResolveService<TSUT>();
        private readonly ServiceProviderFixture<TStartup> _serviceProviderFixture;

        protected IntegrationTest(
            ServiceProviderFixture<TStartup> serviceProviderFixture,
            ITestOutputHelper output)
            : base(output)
        {
            _serviceProviderFixture = serviceProviderFixture;
        }

        protected TService ResolveService<TService>()
        {
            return (TService)_serviceProviderFixture.ServiceProvider.GetRequiredService(typeof(TService));
        }
    }

    public abstract class IntegrationTest : TestBase
    {
        protected readonly DateTime Timestamp;

        protected IntegrationTest(ITestOutputHelper output)
            : base(output)
        {
            TestUsername = "IntegrationTest";
            Timestamp = DateTime.UtcNow;
        }
    }
}

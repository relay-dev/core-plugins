using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit.Abstractions;

namespace Core.Plugins.xUnit.Integration
{
    public abstract class IntegrationTest<TStartup, TSUT> : IntegrationTest where TStartup : IStartup, new()
    {
        private readonly ServiceProviderFixture<TStartup> _serviceProviderFixture;

        protected IntegrationTest(
            ServiceProviderFixture<TStartup> serviceProviderFixture,
            ITestOutputHelper output)
            : base(output)
        {
            _serviceProviderFixture = serviceProviderFixture;
        }

        // Give each test their own instance
        protected TSUT SUT => ResolveService<TSUT>();

        protected TService ResolveService<TService>()
        {
            return (TService)_serviceProviderFixture.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService(typeof(TService));
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

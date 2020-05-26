using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit.Abstractions;


namespace Core.Plugins.xUnit.Integration
{
    public abstract class IntegrationTest<TStartup, TSUT> : TestBase where TStartup : IStartup, new()
    {
        private readonly ServiceProviderFixture<TStartup> _serviceProviderFixture;
        private readonly ITestOutputHelper _output;

        protected IntegrationTest(
            ServiceProviderFixture<TStartup> serviceProviderFixture,
            ITestOutputHelper output)
            : base(output)
        {
            _serviceProviderFixture = serviceProviderFixture;
            _output = output;
        }

        // Give each test their own instance
        protected TSUT SUT => ResolveService<TSUT>();
        protected const string TestUsername = "IntegrationTest";
        protected readonly DateTime Timestamp = DateTime.UtcNow;

        protected TService ResolveService<TService>()
        {
            return (TService)_serviceProviderFixture.ServiceProvider.GetRequiredService(typeof(TService));
        }
    }
}

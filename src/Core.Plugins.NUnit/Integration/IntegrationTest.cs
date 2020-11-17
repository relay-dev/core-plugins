using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using System.Threading;

namespace Core.Plugins.NUnit.Integration
{
    public abstract class IntegrationTest<TSUT> : IntegrationTest
    {
        protected TSUT SUT => (TSUT)CurrentTestProperties.Get(SutKey);

        protected override void BootstrapTest()
        {
            IServiceProvider serviceProvider = Host.Services.CreateScope().ServiceProvider;

            TSUT sut = serviceProvider.GetRequiredService<TSUT>();

            CurrentTestProperties.Set(SutKey, sut);
            CurrentTestProperties.Set(ServiceProviderKey, serviceProvider);
        }

        protected TService ResolveService<TService>()
        {
            var serviceProvider = (IServiceProvider)CurrentTestProperties.Get(ServiceProviderKey);

            return (TService)serviceProvider.GetRequiredService(typeof(TService));
        }

        protected CancellationToken CancellationToken => new CancellationToken();

        protected const string SutKey = "_sut";
        protected const string ServiceProviderKey = "_serviceProvider";
    }

    public abstract class IntegrationTest : TestBase
    {
        protected IHost Host;

        protected IntegrationTest()
        {
            TestUsername = "IntegrationTest";

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("IS_LOCAL", "true");
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Host = Bootstrap();
        }

        [SetUp]
        public void Setup()
        {
            BootstrapTest();
        }

        protected abstract IHost Bootstrap();

        protected virtual void BootstrapTest() { }
    }
}

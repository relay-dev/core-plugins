using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace Core.Plugins.NUnit.Integration
{
    public abstract class IntegrationTest<TSUT> : IntegrationTest
    {
        protected TSUT SUT => (TSUT)CurrentTestProperties.Get(SutKey);

        [SetUp]
        public virtual void Setup()
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

        private const string SutKey = "_sut";
        private const string ServiceProviderKey = "_serviceProvider";
    }

    public abstract class IntegrationTest : TestBase
    {
        protected IHost Host;
        public abstract IHost Bootstrap();

        protected IntegrationTest()
        {
            TestUsername = "IntegrationTest";
        }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Host = Bootstrap();
        }
    }
}

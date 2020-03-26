using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;

namespace Core.Plugins.NUnit.Integration
{
    public abstract class IntegrationTest : TestBase
    {
        protected IHost Host;
        public abstract IHost Bootstrap();

        public IntegrationTest()
        {
            TestUsername = "IntegrationTest";
        }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Host = Bootstrap();
        }
    }

    public abstract class IntegrationTest<TSUT> : IntegrationTest, IDisposable
    {
        protected TSUT SUT
        {
            get
            {
                return ResolveService<TSUT>();
            }
        }

        [SetUp]
        protected virtual void Setup()
        {
            IServiceScope serviceScope = Host.Services.CreateScope();

            CurrentTestProperties.Set(ServiceScopeKey, serviceScope);
        }

        [TearDown]
        protected virtual void TearDown()
        {
            IServiceScope serviceScope = (IServiceScope)CurrentTestProperties.Get(ServiceScopeKey);

            serviceScope.Dispose();
        }

        protected TService ResolveService<TService>()
        {
            IServiceScope serviceScope = (IServiceScope)CurrentTestProperties.Get(ServiceScopeKey);

            return (TService)serviceScope.ServiceProvider.GetRequiredService(typeof(TService));
        }

        public void Dispose()
        {
            TearDown();
        }

        private const string ServiceScopeKey = "_scope";
    }
}

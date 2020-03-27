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

    public abstract class IntegrationTest<TSUT> : IntegrationTest
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
            TSUT sut = Host.Services.CreateScope().ServiceProvider.GetRequiredService<TSUT>();

            CurrentTestProperties.Set(ServiceScopeKey, sut);
        }

        protected TService ResolveService<TService>()
        {
            return (TService)Host.Services.GetRequiredService(typeof(TService));
        }

        private const string ServiceScopeKey = "_scope";
    }
}

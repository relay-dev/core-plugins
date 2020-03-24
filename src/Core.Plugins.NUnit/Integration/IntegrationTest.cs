using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace Common.Testing.Integration
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
                return (TSUT)Host.Services.GetRequiredService(typeof(TSUT));
            }
        }
    }
}

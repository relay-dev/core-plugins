using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

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
                using (var scope = Host.Services.CreateScope())
                {
                    return (TSUT)scope.ServiceProvider.GetRequiredService(typeof(TSUT));
                }
            }
        }
    }
}

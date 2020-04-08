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

    public abstract partial class IntegrationTest<TSUT> : IntegrationTest
    {
        protected TSUT SUT { get; private set; }

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            SUT = Host.Services.CreateScope().ServiceProvider.GetRequiredService<TSUT>();
        }

        protected TService ResolveService<TService>()
        {
            return (TService)Host.Services.GetRequiredService(typeof(TService));
        }
    }
}

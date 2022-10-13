using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Core.Plugins.NUnit.Integration
{
    /// <summary>
    /// A test fixture that contains tests which should run against real dependenies in a local or development environment.
    /// This will run tests in a way that replicates the way the application will be when it's deployed by setting up an IHost to run the tests against.
    /// </summary>
    public abstract class IntegrationTest : TestBase
    {
        /// <summary>
        /// The global IHost that was created by GlobalBootstrap()
        /// </summary>
        protected IHost Host;

        protected IntegrationTest()
        {
            Host = IntegrationTestGlobalContext.Host;
        }

        public override void BootstrapTest()
        {
            base.BootstrapTest();

            // Each test should be given its own service provider to keep the tests isolated. Create a new scope on the IHost and get the service provider
            IServiceProvider serviceProvider = Host.Services.CreateScope().ServiceProvider;

            // Set it on this test's context so we can reference it later to resolve services
            CurrentTestProperties.Set(ServiceProviderKey, serviceProvider);
        }

        /// <summary>
        /// Resolves a service from this test's service provider
        /// </summary>
        protected virtual TService ResolveService<TService>()
        {
            // Get this test's service provider. It was set on the test's current context by the BootstrapTest() method.
            var serviceProvider = (IServiceProvider)CurrentTestProperties.Get(ServiceProviderKey);

            // Use this test's service provider to resolve the service
            return (TService)serviceProvider.GetRequiredService(typeof(TService));
        }

        /// <summary>
        /// Returns a new ILogger everytime it is called
        /// </summary>
        protected virtual ILogger Logger => ResolveService<ILogger>();
        protected const string ServiceProviderKey = "_serviceProvider";
    }

    /// <summary>
    /// A test fixture that contains tests which should run against real dependenies in a local or development environment where you want to speficy the type to test.
    /// This will run tests in a way that replicates the way the application will be when it's deployed by setting up an IHost to run the tests against.
    /// </summary>
    public abstract class IntegrationTest<TSUT> : IntegrationTest
    {
        public override void BootstrapTest()
        {
            base.BootstrapTest();

            // Get this test's service provider. It was set on the test's current context by the BootstrapTest() method.
            var serviceProvider = (IServiceProvider)CurrentTestProperties.Get(ServiceProviderKey);

            // Use this test's service provider to resolve the service of the type we are testing
            TSUT sut = serviceProvider.GetRequiredService<TSUT>();

            // Set the instance on this test's context so we can reference it in SUT
            CurrentTestProperties.Set(SutKey, sut);
        }

        /// <summary>
        /// Returns a new SUT everytime it is called
        /// </summary>
        protected virtual TSUT SUT => (TSUT)CurrentTestProperties.Get(SutKey);

        /// <summary>
        /// Returns a new Logger everytime it is called
        /// </summary>
        protected override ILogger Logger => ResolveService<ILogger<TSUT>>();
        protected const string SutKey = "_sut";
    }
}

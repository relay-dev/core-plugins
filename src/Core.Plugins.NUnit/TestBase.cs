using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;
using System.Threading;

namespace Core.Plugins.NUnit
{
    /// <summary>
    /// Generic test infrastructure for running tests
    /// </summary>
    public abstract class TestBase : TestFrameworkBase
    {
        protected string TestUsername;
        protected DateTime Timestamp;
        protected IPropertyBag CurrentTestProperties;

        protected TestBase()
        {
            TestUsername = GetType().Name;
            Timestamp = DateTime.UtcNow;
            CurrentTestProperties = TestExecutionContext.CurrentContext.CurrentTest.Properties;
        }

        /// <summary>
        /// Called once per TestFixture prior to any child tests
        /// </summary>
        [OneTimeSetUp]
        public virtual void Bootstrap()
        {

        }

        /// <summary>
        /// Called before a Test is run
        /// </summary>
        [SetUp]
        public virtual void BootstrapTest()
        {

        }

        /// <summary>
        /// Called once per TestFixture after all child tests
        /// </summary>
        [OneTimeTearDown]
        public virtual void Cleanup()
        {

        }

        /// <summary>
        /// Called after each time a Test is run
        /// </summary>
        [TearDown]
        public virtual void CleanupTest()
        {

        }

        /// <summary>
        /// Returns a new CancellationToken everytime it is called
        /// </summary>
        protected virtual CancellationToken CancellationToken => new CancellationTokenSource().Token;
    }
}

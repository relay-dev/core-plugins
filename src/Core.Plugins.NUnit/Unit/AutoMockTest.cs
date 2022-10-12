using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using System;

namespace Core.Plugins.NUnit.Unit
{
    /// <summary>
    /// Test infrastructure for running unit tests where you want to automatically mock all arguments in the constructor of the type you are testing
    /// </summary>
    /// <remarks>Auto-mocking is what gives us the freedom to change constructor parameters without breaking existing tests</remarks>
    public abstract class AutoMockTest<TCUT> : TestBase where TCUT : class
    {
        protected virtual TCUT CUT => (TCUT)CurrentTestProperties.Get(CutKey);

        protected AutoMockTest()
        {
            TestUsername = "AutoMockTest";
        }

        public override void BootstrapTest()
        {
            base.BootstrapTest();

            // Each test should be given its own AutoMocker instance to keep the tests isolated
            var autoMocker = new AutoMocker();

            // Use it to create an instance of TCUT with all of it's contructor dependencies populated with mocks
            TCUT cut = autoMocker.CreateInstance<TCUT>();

            // Set the instance of TCUT and the AutoMocker on this test's context so we can reference it later (cut is accessed by individual tests and autoMocker is used to retrieve the mocks it created later)
            CurrentTestProperties.Set(CutKey, cut);
            CurrentTestProperties.Set(ContainerKey, autoMocker);
        }

        /// <summary>
        /// Gets the mock of the dependency of the type under test that was automatically created during Bootstrap().
        /// You can use this if you want to setup the mocks before running your tests.
        /// </summary>
        protected virtual Mock<TMock> ResolveMock<TMock>() where TMock : class
        {
            // Get this test's AutoMock instance. It was set on the test's current context by the BootstrapTest() method.
            AutoMocker autoMocker = (AutoMocker)CurrentTestProperties.Get(ContainerKey);

            if (autoMocker == null)
            {
                throw new InvalidOperationException("AutoMocker not initialized");
            }

            // Use this test's AutoMock instance to get the mock of the type requested
            return autoMocker.GetMock<TMock>();
        }

        /// <summary>
        /// Verifies that the mock of the ILogger was called
        /// </summary>
        /// <remarks>The way you have to do this is kind of painful because there is really only method being called on the ILogger, and it has a signature to support all use-cases. The extension methods we usually use in code can't be used to verify, so this method simplifies the process</remarks>
        protected virtual void VerifyLoggerWasCalled(Mock<ILogger> logger, LogLevel logLevel, string expectedMessage, Times times)
        {
            Func<object, Type, bool> state = (v, t) => v.ToString().Contains(expectedMessage);

            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == logLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), times);
        }

        private const string CutKey = "_cut";
        private const string ContainerKey = "_container";
    }
}

using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;

namespace Core.Plugins.NUnit.Unit
{
    public class AutoMockTest<TCUT> : TestBase where TCUT : class
    {
        protected virtual TCUT CUT => (TCUT)CurrentTestProperties.Get(CutKey);

        public AutoMockTest()
        {
            TestUsername = "AutoMockTest";
        }

        [SetUp]
        public void Setup()
        {
            BootstrapTest();
        }

        protected virtual Mock<TMock> ResolveMock<TMock>() where TMock : class
        {
            AutoMocker autoMocker = (AutoMocker)CurrentTestProperties.Get(ContainerKey);

            return autoMocker.GetMock<TMock>();
        }

        protected virtual void BootstrapTest()
        {
            var autoMocker = new AutoMocker();

            TCUT cut = autoMocker.CreateInstance<TCUT>();

            CurrentTestProperties.Set(CutKey, cut);
            CurrentTestProperties.Set(ContainerKey, autoMocker);
        }

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

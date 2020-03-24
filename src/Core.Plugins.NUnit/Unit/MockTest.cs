using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Collections.Generic;

namespace Core.Plugins.NUnit.Unit
{
    public class MockTest<TCUT> : TestBase where TCUT : class
    {
        public MockTest()
        {
            TestUsername = "UnitTest";
        }

        protected TCUT CUT
        {
            get
            {
                return (TCUT)CurrentTestProperties.Get(CutKey);
            }
        }

        [SetUp]
        public virtual void Setup()
        {
            var autoMocker = new AutoMocker();

            TCUT cut = autoMocker.CreateInstance<TCUT>();

            CurrentTestProperties.Set(CutKey, cut);
            CurrentTestProperties.Set(ContainerKey, autoMocker);
        }

        protected Mock<TMock> ResolveMock<TMock>() where TMock : class
        {
            AutoMocker autoMocker = (AutoMocker)CurrentTestProperties.Get(ContainerKey);

            return autoMocker.GetMock<TMock>();
        }

        protected IPropertyBag CurrentTestProperties
        { 
            get
            {
                return TestExecutionContext.CurrentContext.CurrentTest.Properties;
            }
        }

        private const string CutKey = "_cut";
        private const string ContainerKey = "_container";
    }
}

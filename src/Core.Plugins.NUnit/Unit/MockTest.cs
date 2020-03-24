using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Collections.Generic;

namespace Common.Testing.Unit
{
    public partial class MockTest<TCUT> : TestBase where TCUT : class
    {
        public MockTest()
        {
            TestUsername = "UnitTest";
        }

        public TCUT CUT
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
            CurrentTestProperties.Set(RepositoryKey, new Dictionary<string, object>());
        }

        public Mock<TMock> ResolveMock<TMock>() where TMock : class
        {
            AutoMocker autoMocker = (AutoMocker)CurrentTestProperties.Get(ContainerKey);

            return autoMocker.GetMock<TMock>();
        }

        private IPropertyBag CurrentTestProperties
        { 
            get
            {
                return TestExecutionContext.CurrentContext.CurrentTest.Properties;
            }
        }

        private const string CutKey = "_cut";
        private const string ContainerKey = "_container";
        private const string RepositoryKey = "_repository";
    }
}

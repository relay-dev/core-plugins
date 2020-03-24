using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Collections.Generic;

namespace Core.Plugins.NUnit.Unit
{
    public partial class MockTest<TCUT> : TestBase where TCUT : class
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
            CurrentTestProperties.Set(RepositoryKey, new Dictionary<string, object>());
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

        protected const string CutKey = "_cut";
        protected const string ContainerKey = "_container";
        protected const string RepositoryKey = "_repository";
    }
}

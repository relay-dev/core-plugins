﻿using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace Core.Plugins.NUnit.Unit
{
    public class AutoMockTest<TCUT> : TestBase where TCUT : class
    {
        protected TCUT CUT => (TCUT)CurrentTestProperties.Get(CutKey);

        public AutoMockTest()
        {
            TestUsername = "AutoMockTest";
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

        private const string CutKey = "_cut";
        private const string ContainerKey = "_container";
    }
}

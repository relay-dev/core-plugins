using Moq.AutoMock;
using Xunit.Abstractions;

namespace Core.Plugins.xUnit.Unit
{
    public class AutoMockTest<TCUT> : TestBase where TCUT : class
    {
        protected readonly AutoMocker AutoMocker;
        protected TCUT CUT => AutoMocker.CreateInstance<TCUT>();

        public AutoMockTest(ITestOutputHelper output)
            : base(output)
        {
            TestUsername = "AutoMockTest";
            AutoMocker = new AutoMocker();
        }
    }
}

using Xunit.Abstractions;

namespace UnitTests.Base
{
    public class xUnitTestBase : UnitTestBase
    {
        private readonly ITestOutputHelper _output;

        public xUnitTestBase(ITestOutputHelper output)
        {
            _output = output;
        }

        protected override void Output(string s)
        {
            _output.WriteLine(s);
        }

        protected override void Output(object o)
        {
            _output.WriteLine(SerializeToString(o));
        }
    }
}

using Xunit.Abstractions;

namespace IntegrationTests.Base
{
    public class xUnitIntegrationTestBase : IntegrationTestBase
    {
        private readonly ITestOutputHelper _output;

        public xUnitIntegrationTestBase(ITestOutputHelper output)
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

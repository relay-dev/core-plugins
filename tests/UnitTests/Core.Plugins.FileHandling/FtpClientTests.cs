using Core.Plugins.FileHandling.FTP;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Core.Plugins.FileHandling
{
    public class FtpClientTests : xUnitTestBase
    {
        public FtpClientTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldParseConnectionStringAsExpected_WhenConnectionStringIsValid()
        {
            string input = "Address=ftp.totalims.com;User ID=GAP;Password=ran798;";

            var ftpClient = new FtpClient(input);
        }
    }
}

using Core.Plugins.Communication.Ftp;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Core.Plugins
{
    public class FtpClientSettingsTests : xUnitTestBase
    {
        public FtpClientSettingsTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldParseConnectionStringAsExpected_WhenConnectionStringIsValid()
        {
            string input = "Host=guroosolutions.com;Username=admin@guroosolutions.com;Password=R8u3y6wnfUeNG52dsSuy;TimeoutInSeconds=1800;IsSftp=false;";

            var settings = new FtpClientSettings(input);

            Assert.NotNull(settings.Host);
            Assert.NotNull(settings.Username);
            Assert.NotNull(settings.Password);
        }
    }
}

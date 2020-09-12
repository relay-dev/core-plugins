using Core.Plugins.Ftp;
using Core.Plugins.xUnit;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Core.Plugins
{
    public class FtpClientSettingsTests : TestBase
    {
        public FtpClientSettingsTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldParseConnectionStringAsExpected_WhenConnectionStringIsValid()
        {
            string input = "Host=host.com;Username=username;Password=password;TimeoutInSeconds=1800;IsSftp=false;";

            var settings = new FtpClientSettings(input);

            Assert.NotNull(settings.Host);
            Assert.NotNull(settings.Username);
            Assert.NotNull(settings.Password);
        }
    }
}

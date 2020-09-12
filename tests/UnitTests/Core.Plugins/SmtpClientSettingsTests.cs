using Core.Plugins.Smtp;
using Core.Plugins.xUnit;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Core.Plugins
{
    public class SmtpClientSettingsTests : TestBase
    {
        public SmtpClientSettingsTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldParseConnectionStringAsExpected_WhenConnectionStringIsValid()
        {
            string input = "Host=ftp.testing.com;Port=20;Username=ABC;Password=123456;";

            var settings = new SmtpClientSettings(input);

            Assert.NotNull(settings.Host);
            Assert.NotNull(settings.Port);
            Assert.NotNull(settings.Username);
            Assert.NotNull(settings.Password);
        }
    }
}

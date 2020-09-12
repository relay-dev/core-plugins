using Core.Plugins.Ftp;
using Core.Plugins.xUnit;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Core.Plugins.FileHandling
{
    public class FtpClientTests : TestBase
    {
        public FtpClientTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldParseConnectionStringAsExpected_WhenConnectionStringIsValid()
        {
            string input = "Host=sftp.testing.com;Username=ABC;Password=123456;IsSftp=true";

            var ftpClientSettings = new FtpClientSettings(input);

            Assert.NotNull(ftpClientSettings.Host);
            Assert.NotNull(ftpClientSettings.Username);
            Assert.NotNull(ftpClientSettings.Password);
            Assert.True(ftpClientSettings.IsSftp);
        }

        [Fact]
        public void Constructor_ShouldDefaultIsSftp_WhenConnectionStringOmmitsTheProperty()
        {
            string input = "Host=ftp.totalims.com;Username=GAP;Password=ran798;";

            var ftpClientSettings = new FtpClientSettings(input);

            Assert.NotNull(ftpClientSettings.Host);
            Assert.NotNull(ftpClientSettings.Username);
            Assert.NotNull(ftpClientSettings.Password);
            Assert.False(ftpClientSettings.IsSftp);
        }
    }
}

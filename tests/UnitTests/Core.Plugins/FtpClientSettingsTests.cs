using Core.Plugins.Ftp;
using Core.Plugins.NUnit;
using NUnit.Framework;
using Shouldly;

namespace UnitTests.Core.Plugins
{
    [TestFixture]
    public class FtpClientSettingsTests : TestBase
    {
        [Test]
        public void Constructor_ShouldParseConnectionStringAsExpected_WhenConnectionStringIsValid()
        {
            // Arrange
            string input = "Host=host.com;Username=username;Password=password;TimeoutInSeconds=1800;IsSftp=false;";

            // Act
            var settings = new FtpClientSettings(input);

            // Assert
            settings.Host.ShouldNotBeNull();
            settings.Host.ShouldBe("host.com");
            settings.Username.ShouldNotBeNull();
            settings.Username.ShouldBe("username");
            settings.Password.ShouldNotBeNull();
            settings.Password.ShouldBe("password");
        }
    }
}

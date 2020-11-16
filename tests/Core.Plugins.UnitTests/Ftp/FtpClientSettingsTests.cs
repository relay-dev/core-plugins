using Core.Plugins.Ftp;
using Core.Plugins.NUnit;
using NUnit.Framework;
using Shouldly;

namespace Core.Plugins.UnitTests.Ftp
{
    [TestFixture]
    public class FtpClientSettingsTests : TestBase
    {
        [Test]
        public void Constructor_ShouldParseConnectionStringAsExpected_WhenConnectionStringIsValid()
        {
            // Arrange
            string input = "Host=sftp.testing.com;Port=20;Username=ABC;Password=123456;IsSftp=true";

            // Act
            var settings = new FtpClientSettings(input);

            // Assert
            settings.Host.ShouldNotBeNull();
            settings.Host.ShouldBe("sftp.testing.com");
            settings.Port.ShouldNotBeNull();
            settings.Port.ShouldBe("20");
            settings.Username.ShouldNotBeNull();
            settings.Username.ShouldBe("ABC");
            settings.Password.ShouldNotBeNull();
            settings.Password.ShouldBe("123456");
            settings.IsSftp.ShouldBe(true);
        }

        [Test]
        public void Constructor_ShouldDefaultIsSftp_WhenConnectionStringOmitsTheProperty()
        {
            // Arrange
            string input = "Host=sftp.testing.com;Username=ABC;Password=123456";

            // Act
            var settings = new FtpClientSettings(input);

            // Assert
            settings.Host.ShouldNotBeNull();
            settings.Host.ShouldBe("sftp.testing.com");
            settings.Username.ShouldNotBeNull();
            settings.Username.ShouldBe("ABC");
            settings.Password.ShouldNotBeNull();
            settings.Password.ShouldBe("123456");
            settings.IsSftp.ShouldBe(false);
        }
    }
}

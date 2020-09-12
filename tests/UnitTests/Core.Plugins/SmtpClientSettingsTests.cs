using Core.Plugins.NUnit;
using Core.Plugins.Smtp;
using NUnit.Framework;
using Shouldly;

namespace UnitTests.Core.Plugins
{
    [TestFixture]
    public class SmtpClientSettingsTests : TestBase
    {
        [Test]
        public void Constructor_ShouldParseConnectionStringAsExpected_WhenConnectionStringIsValid()
        {
            // Arrange
            string input = "Host=ftp.testing.com;Port=20;Username=ABC;Password=123456;";

            // Act
            var settings = new SmtpClientSettings(input);

            // Assert
            settings.Host.ShouldNotBeNull();
            settings.Host.ShouldBe("ftp.testing.com");
            settings.Port.ShouldNotBeNull();
            settings.Port.ShouldBe("20");
            settings.Username.ShouldNotBeNull();
            settings.Username.ShouldBe("ABC");
            settings.Password.ShouldNotBeNull();
            settings.Password.ShouldBe("123456");
        }
    }
}

﻿using Core.Communication.Smtp;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Core.Plugins
{
    public class SmtpClientTests : xUnitTestBase
    {
        public SmtpClientTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldParseConnectionStringAsExpected_WhenConnectionStringIsValid()
        {
            string input = "Host=ftp.testing.com;Port=20;Username=ABC;Password=123456;";

            var ftpClientSettings = new SmtpClientSettings(input);

            Assert.NotNull(ftpClientSettings.Host);
            Assert.NotNull(ftpClientSettings.Port);
            Assert.NotNull(ftpClientSettings.Username);
            Assert.NotNull(ftpClientSettings.Password);
        }
    }
}

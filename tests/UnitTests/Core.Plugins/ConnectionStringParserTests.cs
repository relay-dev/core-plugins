using Core.Plugins.Application;
using Core.Plugins.Extensions;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Core.Plugins
{
    public class ConnectionStringParserTests : xUnitTestBase
    {
        public ConnectionStringParserTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void GetConnectionString_ShouldReplaceAllPlaceholders_WhenPlaceholdersExist()
        {
            // Arrange
            const string connectionString = "SegmentOne=ValueOne;SegmentTwo=ValueTwo;";
            
            // Act
            dynamic parsed = new ConnectionStringParser().Parse(connectionString).ToDynamic();

            // Assert
            Assert.NotNull(parsed);
            Assert.NotNull(parsed);
            Assert.NotNull(parsed.SegmentOne);
            Assert.Equal("ValueOne", parsed.SegmentOne);
            Assert.NotNull(parsed.SegmentTwo);
            Assert.Equal("ValueTwo", parsed.SegmentTwo);
        }

        [Fact]
        public void GetConnectionString_ShouldParseAsExpected_WhenEqualSignIsInSegment()
        {
            // Arrange
            const string connectionString = "Host=SomeURL;Key=12345=;";
            
            // Act
            dynamic parsed = new ConnectionStringParser().Parse(connectionString).ToDynamic();

            // Assert
            Assert.NotNull(parsed);
            Assert.NotNull(parsed);
            Assert.NotNull(parsed.Host);
            Assert.Equal("SomeURL", parsed.SegmentOne);
            Assert.NotNull(parsed.Key);
            Assert.Equal("12345=", parsed.SegmentTwo);
        }
    }
}

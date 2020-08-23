using Core.Plugins.Application;
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
            var parser = new ConnectionStringParser(connectionString);

            // Assert
            Assert.NotNull(parser);
            Assert.NotNull(parser.Segment);
            Assert.NotNull(parser.Segment.SegmentOne);
            Assert.Equal("ValueOne", parser.Segment.SegmentOne);
            Assert.NotNull(parser.Segment.SegmentTwo);
            Assert.Equal("ValueTwo", parser.Segment.SegmentTwo);
        }
    }
}

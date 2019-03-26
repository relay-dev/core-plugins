using Core.Plugins.Utilities;
using System;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Utilities
{
    public class GlobalHelperTests : xUnitTestBase
    {
        public GlobalHelperTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void IsAnyStringPopulatedShouldReturnFalseIfAllStringsAreNullOrEmpty()
        {
            bool result = GlobalHelper.IsAnyStringPopulated(null, String.Empty, "");

            Assert.False(result);
        }

        [Fact]
        public void IsAnyStringPopulatedShouldReturnTrueIfAllStringsAreNotNullOrEmpty()
        {
            bool result = GlobalHelper.IsAnyStringPopulated("A", "B", "ABC");

            Assert.True(result);
        }

        [Fact]
        public void IsAnyStringPopulatedShouldReturnTrueIfSomeStringsAreNotNullButOthersAreNull()
        {
            bool result = GlobalHelper.IsAnyStringPopulated(null, String.Empty, "Populated!");

            Assert.True(result);
        }
    }
}

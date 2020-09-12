using Core.Plugins.Caching;
using Core.Plugins.NUnit.Unit;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using NUnit.Framework;

namespace UnitTests.Core.Plugins
{
    [TestFixture]
    public class DistributedCacheTests : AutoMockTest<DistributedCache>
    {
        [Test]
        public void GetOrSet_ShouldCallSet_WhenThereIsNoMatchOnKey()
        {
            // Arrange
            const int valueToCache = 100;

            // Act
            CUT.GetOrSet("CacheKey", () => valueToCache);

            // Assert
            ResolveMock<IDistributedCache>()
                .Verify(mock => mock.Set("CacheKey", It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()));
        }
    }
}

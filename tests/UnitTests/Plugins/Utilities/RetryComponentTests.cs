using Core.Plugins.Utilities;
using Core.Utilities;
using System;
using System.Linq;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Plugins.Utilities
{
    public class RetryComponentTests : xUnitTestBase
    {
        public RetryComponentTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void RetryAction_ShouldRetryTheAmountOfTimesAsSpecified_WhenRetryCountIsGreaterThanZero()
        {
            const int timesToTry = 5;
            Action action = () => { throw new Exception("Boom!"); };

            RetryResult retryResult = CUT.RetryAction(timesToTry, TimeSpan.FromMilliseconds(1), action);

            Assert.NotNull(retryResult);
            Assert.Equal(timesToTry, retryResult.TimesTried);
        }

        [Fact]
        public void RetryAction_ShouldReturnAllExceptionsThrown_WhenAnyExceptionIsThrown()
        {
            const int timesToTry = 5;
            Action action = () => { throw new Exception("Boom!"); };

            RetryResult retryResult = CUT.RetryAction(timesToTry, TimeSpan.FromMilliseconds(1), action);

            Assert.NotNull(retryResult);
            Assert.NotNull(retryResult.Exceptions);
            Assert.Equal(timesToTry, retryResult.Exceptions.Count);
        }

        [Fact]
        public void RetryAction_ShouldReturnTheCorrectExceptionsThrown_WhenAnyExceptionIsThrown()
        {
            const int timesToTry = 5;
            Action action = () => { throw new Exception("Boom!"); };

            RetryResult retryResult = CUT.RetryAction(timesToTry, TimeSpan.FromMilliseconds(1), action);

            Assert.NotNull(retryResult);
            Assert.NotNull(retryResult.Exceptions);
            Assert.True(retryResult.Exceptions.All(e => e.Message == "Boom!"));
        }

        [Fact]
        public void RetryAction_ShouldOnlyExecuteOnceIfNoExceptionsAreThrown_WhenAnyExceptionIsThrown()
        {
            const int timesToTry = 5;
            Action action = () => { };

            RetryResult retryResult = CUT.RetryAction(timesToTry, TimeSpan.FromMilliseconds(1), action);

            Assert.NotNull(retryResult);
            Assert.Equal(1, retryResult.TimesTried);
        }

        [Fact]
        public void RetryAction_ShouldReturnTheExpectedValue_WhenAnyExceptionIsThrown()
        {
            const int timesToTry = 5;
            Func<int> func = () => { return 100; };

            RetryResult<int> retryResult = CUT.RetryFunc(timesToTry, TimeSpan.FromMilliseconds(1), func);

            Assert.NotNull(retryResult);
            Assert.Equal(1, retryResult.TimesTried);
            Assert.Equal(100, retryResult.Result);
        }

        private RetryComponent CUT => new RetryComponent();
    }
}

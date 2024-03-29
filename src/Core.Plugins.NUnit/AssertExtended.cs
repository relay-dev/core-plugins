﻿using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace Core.Plugins.NUnit
{
    public static class AssertExtended
    {
        /// <summary>
        /// Verifies that the mock of an ILogger was called
        /// </summary>
        /// <remarks>The way you have to do this is kind of painful because there is really only ever one method being called on the ILogger. The extension methods we usually use in code can't be used to verify, so we need to check the underlying method being called.</remarks>
        public static void LoggerWasCalled(Mock<ILogger> logger, LogLevel logLevel, string expectedMessage, Times times)
        {
            Func<object, Type, bool> state = (v, t) => v.ToString().Contains(expectedMessage);

            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == logLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), times);
        }
    }
}

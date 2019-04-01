using Core.Framework.Attributes;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Core.Plugins.Utilities
{
    [Component]
    [Injectable]
    public class RetryComponent : IRetryComponent
    {
        public RetryResult RetryAction(int timesToRetry, TimeSpan retryInterval, Action action)
        {
            Func<object> func = () =>
            {
                action();
                return null;
            };

            return RetryFunc(timesToRetry, retryInterval, func);
        }

        public RetryResult<T> RetryFunc<T>(int timesToRetry, TimeSpan retryInterval, Func<T> func)
        {
            if (timesToRetry < 1)
                throw new ArgumentException("The timesToRetry parameter must be a positive integer", "timesToRetry");

            int timesTried;
            var retryResult = new RetryResult<T>();

            for (timesTried = 0; timesTried < timesToRetry; timesTried++)
            {
                try
                {
                    if (timesTried > 0)
                        Thread.Sleep(retryInterval);

                    retryResult.Result = func.Invoke();

                    retryResult.IsSuccess = true;
                    retryResult.TimesTried = timesTried + 1;

                    return retryResult;
                }
                catch (Exception ex)
                {
                    if (retryResult.Exceptions == null)
                    {
                        retryResult.Exceptions = new List<Exception>();
                    }

                    retryResult.Exceptions.Add(ex);
                }
            }

            retryResult.IsSuccess = false;
            retryResult.TimesTried = timesTried;

            return retryResult;
        }
    }
}

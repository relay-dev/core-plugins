using Core.Providers;
using System;

namespace Core.Plugins.Providers
{
    public class RandomLongProvider : IRandomLongProvider
    {
        public long Get()
        {
            ulong uRange = ulong.MaxValue;

            ulong ulongRand;

            do
            {
                byte[] buf = new byte[8];

                new Random(Guid.NewGuid().GetHashCode()).NextBytes(buf);

                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange);
        }

        public long Get(long minValue, long maxValue)
        {
            if (minValue < 0)
            {
                throw new ArgumentException("minValue cannot be less than 0", nameof(minValue));
            }

            if (maxValue <= 0)
            {
                throw new ArgumentException("maxValue cannot be less than or equal to 0", nameof(maxValue));
            }

            if (maxValue < minValue)
            {
                throw new ArgumentException("maxValue cannot be less than minValue", nameof(maxValue));
            }

            ulong uRange = (ulong)(maxValue - minValue);

            ulong ulongRand;

            do
            {
                byte[] buf = new byte[8];

                new Random(Guid.NewGuid().GetHashCode()).NextBytes(buf);

                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + minValue;
        }
    }
}

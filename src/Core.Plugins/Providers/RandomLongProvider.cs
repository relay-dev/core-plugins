using Core.Framework;
using Core.Providers;
using System;

namespace Core.Plugins.Providers
{
    [Component]
    [Injectable]
    internal class RandomLongProvider : IRandomLongProvider
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
    }
}

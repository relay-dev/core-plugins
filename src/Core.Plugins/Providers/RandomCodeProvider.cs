using Core.Framework;
using Core.Providers;
using System;

namespace Core.Plugins.Providers
{
    [Component]
    [Injectable]
    public class RandomCodeProvider : IRandomCodeProvider
    {
        private readonly IRandomLongProvider _randomLongProvider;

        public RandomCodeProvider(IRandomLongProvider randomLongProvider)
        {
            _randomLongProvider = randomLongProvider;
        }

        public string Get(int length, string prefix = null, string suffix = null)
        {
            if (length <= 0)
            {
                throw new ArgumentException("length cannot be less than or equal to 0", nameof(length));
            }

            prefix ??= string.Empty;
            suffix ??= string.Empty;

            int lengthOfNumericSubstring = length - prefix.Length - suffix.Length;

            if (lengthOfNumericSubstring > 18)
            {
                throw new ArgumentException("The numeric portion of the Confirmation Number cannot have a length longer than 18", nameof(length));
            }

            long minValue = long.Parse("1" + string.Empty.PadRight(lengthOfNumericSubstring - 1, '0'));
            long maxValue = long.Parse("9" + string.Empty.PadRight(lengthOfNumericSubstring - 1, '9'));

            long randomNumber = _randomLongProvider.Get(minValue, maxValue);

            return $"{prefix}{randomNumber}{suffix}";
        }
    }
}

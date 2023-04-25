using System;
using System.Globalization;

namespace Org.Front.Core.Contracts.Extensions
{
    public static class DecimalExtensions
    {
        private static readonly CultureInfo usCulture = new CultureInfo("en-us");

        public static decimal ToDecimal(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0M;
            }

            return decimal.Parse(value, usCulture);
        }

        public static decimal? ToNullableDecimal(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return (decimal?) null;
            }

            if(decimal.TryParse(value, NumberStyles.Any, usCulture, out decimal result))
            {
                return result;
            }

            return (decimal?)null;
        }

        public static bool TryToDecimal(this string input, out decimal output)
        {
            output = 0;
            var canParse = decimal.TryParse(input, NumberStyles.Any, usCulture, out var value);

            if (canParse)
            {
                output = value;
            }

            return canParse;
        }
        
        public static decimal ToDecimalOrDefault(this string input) =>
            decimal.TryParse(input, NumberStyles.Any, usCulture, out decimal value) ?  value :  0M;
        
        public static decimal ToModule(this decimal input) => Math.Abs(input);

        public static int GetDecimalPlaces(this decimal input) =>
            BitConverter.GetBytes(decimal.GetBits(input)[3])[2];

        public static decimal RemoveTrailingZeroes(this decimal value)
            => value / 1.000000000000000000000000000000000m;
    }
}

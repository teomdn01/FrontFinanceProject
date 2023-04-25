namespace Brokers.Coinbase.Extensions
{
    public static class CoinbaseStringExtensions
    {
        public static T ParseCoinbaseStringToEnum<T>(this string source) where T : Enum
        {
            if (source == null)
            {
                return default;
            }

            source = source.Replace("_", "");
            if (Enum.TryParse(typeof(T), source, true, out var result))
            {
                return (T)result;
            }

            return default;
        }
    }
}

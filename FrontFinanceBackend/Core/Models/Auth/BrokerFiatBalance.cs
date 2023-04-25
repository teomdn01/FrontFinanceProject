namespace Org.Front.Core.Contracts.Models.Brokers
{
    public class BrokerFiatBalance
    {
        /// <summary>
        /// Account balance currency
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// BuyingPower indicates total amount of money the user can spend for buying stock. Always includes cash and
        /// can also include margin
        /// </summary>
        public decimal? BuyingPower { get; set; }

        /// <summary>
        /// BuyingPower indicates total amount of money the user can spend for buying crypto.
        /// </summary>
        public decimal? CryptoBuyingPower { get; set; }

        /// <summary>
        /// Account cash indicates total amount of money
        /// </summary>
        public decimal? Cash { get; set; }
    }
}

using System.Runtime.InteropServices;

namespace Org.Front.Core.Contracts.Models.Brokers
{
    public class BrokerGetAuthenticationLinkRequest
    {
        /// <summary>
        /// Name of the link to use. Currently used for Plaid only and allows to
        /// request a pre-configured Plaid Link UI to be shown to the user
        /// (e.g. with some set of banks or just one bank)
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        /// Optional redirect link URL for broker with oAuth authentication. If not used,
        /// the default redirect URL configured per a broker is used
        /// </summary>
        public string RedirectLink { get; set; }
        
        public bool EnableCryptocurrencyTransfers { get; set; }
    }
}

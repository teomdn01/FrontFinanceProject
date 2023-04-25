using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace Org.Front.Core.Contracts.Models.Brokers
{
    public class BrokerRefreshTokenRequest : BrokerBaseAuthRequest
    {
        [Required]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Optional, used when we the refresh token should be refreshed.
        /// Currently this flow is supported by TD Ameritrade
        /// </summary>
        public bool? CreateNewRefreshToken { get; set; }

        /// <summary>
        /// Some institutions may require accessToken to be provided as well.
        /// It's currently required by WeBull only
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Currently used to update WeBull trade token.
        /// </summary>
        public string TradeToken { get; set; }

        /// <summary>
        /// Additional metadata
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class BrokerBaseRequest
{
    /// <summary>
    /// Auth token that allows connecting to the target institution
    /// </summary>
    public string AuthToken { get; set; }

    /// <summary>
    /// Type of the institution to connect
    /// </summary>
    [Required]
    public BrokerType Type { get; set; }
}
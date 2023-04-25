using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class BrokerBaseAuthRequest
{
    [Required]
    public BrokerType Type { get; set; }
}
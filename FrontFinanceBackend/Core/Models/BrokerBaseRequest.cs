using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class BrokerBaseRequest
{
    [Required]
    public BrokerType Type { get; set; }
}
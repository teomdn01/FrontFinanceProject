using System.Text.Json.Serialization;

namespace Brokers.Trading212.Models;

public class AuthenticatePayload
{
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
    [JsonPropertyName("rememberMe")]
    public bool RememberMe { get; set; }
}
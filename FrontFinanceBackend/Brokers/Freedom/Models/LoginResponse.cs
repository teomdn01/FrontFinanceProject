namespace Brokers.Freedom.Models;

public class LoginResponse
{
    public bool success { get; set; }
    public bool logged { get; set; }
    public string SID { get; set; }
    public int userId { get; set; }
    public bool real { get; set; }
    public string account_type { get; set; }
    public string account_title { get; set; }
    public object account_type_description { get; set; }
}
namespace Brokers.Tradier.Models;

public class GetUserProfileResponse
{
    public Profile profile { get; set; }
}

public class Account
{
    public string account_number { get; set; }
    public string classification { get; set; }
    public DateTime date_created { get; set; }
    public bool day_trader { get; set; }
    public int option_level { get; set; }
    public string status { get; set; }
    public string type { get; set; }
    public DateTime last_update_date { get; set; }
}

public class Profile
{
    public Account account { get; set; }
    public string id { get; set; }
    public string name { get; set; }
}




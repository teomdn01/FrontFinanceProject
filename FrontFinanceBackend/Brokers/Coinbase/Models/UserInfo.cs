namespace Brokers.Coinbase.Models
{
    public class UserInfo
    {
        public UserData Data { get; set; }
    }

    public class UserData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Resource { get; set; }
        public string ResourcePath { get; set; }
    }
}

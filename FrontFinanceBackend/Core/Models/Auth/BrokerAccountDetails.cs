namespace Org.Front.Core.Contracts.Models.Brokers
{
    public class BrokerAccountDetails
    {
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public string PrimaryEmail { get; set; }
        public string AdditionalEmails { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string AdditionalPhoneNumbers { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AdditionalAddresses { get; set; }
        public string Region { get; set; }
        public string Citizenship { get; set; }
        public string Address { get; set; }
        public string RiskLevelText { get; set; }
        public BrokerUserSex? Sex { get; set; }
        public long? DateOfBirthTimestamp { get; set; }
        public long? AccountCreatedTimestamp { get; set; }
        public string AccountName { get; set; }
        public string AccountMask { get; set; }
    }
}

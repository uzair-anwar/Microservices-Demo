namespace Users.Data.Models
{
    public class UserInfo
    {
        public string Sub { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string PreferredUsername { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Zoneinfo { get; set; }
        public long UpdatedAt { get; set; }
        public bool EmailVerified { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string Locality { get; set; }
        public string Country { get; set; }
    }
}

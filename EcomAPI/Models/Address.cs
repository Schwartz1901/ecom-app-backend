using static EcomAPI.Models.UserProfile;

namespace EcomAPI.Models
{
    public class Address
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public UserProfile User { get; set; }

        public string RecipientName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public bool IsDefault { get; set; }
    }
}

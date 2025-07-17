using AuthAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EcomAPI.Models
{
    public class UserProfile
    {
        
        public Guid Id { get; set; } = Guid.NewGuid();  // Internal primary key

        // Foreign key to Identity user (AspNetUsers.Id)
        public string UserId { get; set; }
        // Navigation to ApplicationUser, Never send back
        [JsonIgnore]
        public ApplicationUser AuthUser { get; set; }

        // Profile data
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
  
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Quote { get; set; }
        public string? SubName { get; set; }

        // Create Datetime
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Collections
        public ICollection<Address> Addresses { get; set; } = new List<Address>();


    }
}

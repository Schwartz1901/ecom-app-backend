using AuthAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcomAPI.Models
{
    public class UserProfile
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();  // Internal primary key

        [Required]
        public string UserId { get; set; }

        // Navigation to AspNetUsers
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser AuthUser { get; set; }

        // Profile data
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
<<<<<<< HEAD
  
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Quote { get; set; }
        public string? SubName { get; set; }
=======

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Url]
        public string AvatarUrl { get; set; } = string.Empty;

        [StringLength(200)]
        public string Quote { get; set; } = string.Empty;

        [StringLength(50)]
        public string SubName { get; set; } = string.Empty;
>>>>>>> b81058516da844056de0e627b62ad2532d9d3108

        // Audit info
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation collection
        public ICollection<Address> Addresses { get; set; } = new List<Address>();


    }
}

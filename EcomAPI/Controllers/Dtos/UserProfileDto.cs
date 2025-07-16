public class UserProfileDto
{
    public Guid Id { get; set; }                  // Internal profile ID
    public string Username { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
         
    public string PhoneNumber { get; set; }
    public string AvatarUrl { get; set; }
    public string Quote { get; set; }
    public string SubName { get; set; }


    public DateTime CreatedAt { get; set; }
}

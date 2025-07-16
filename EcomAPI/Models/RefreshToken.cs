using AuthAPI.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; } = false;

    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public bool IsActive => !IsRevoked && !IsExpired;
}

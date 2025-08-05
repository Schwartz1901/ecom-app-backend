namespace AuthService.API.DTOs
{
    public class LogoutDto
    {
        public Guid Id { get; set; }
        public string RefreshToken { get; set; }
    }
}

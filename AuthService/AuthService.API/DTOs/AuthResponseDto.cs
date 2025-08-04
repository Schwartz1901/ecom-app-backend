namespace AuthService.API.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }  
        public string AuthId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}

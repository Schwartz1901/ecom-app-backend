namespace AuthAPI.Controllers.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}

namespace User.API.DTOs
{
    public class CreateUserRequestDto
    {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}

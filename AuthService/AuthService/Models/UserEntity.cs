using Microsoft.AspNetCore.Identity;

namespace AuthService.Models
{
    public class UserEntity: IdentityUser
    {
        public string Item {  get; set; }
    }
}

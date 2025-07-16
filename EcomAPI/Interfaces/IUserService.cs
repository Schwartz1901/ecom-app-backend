using AuthAPI.Models;
using EcomAPI.Models;

namespace EcomAPI.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetUserAsync(Guid id);
        Task PostUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(Guid id);
    }
}

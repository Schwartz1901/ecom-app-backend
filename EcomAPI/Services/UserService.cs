using AuthAPI;
using AuthAPI.Models;
using EcomAPI.Interfaces;
using EcomAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcomAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<UserProfileDto> GetUserAsync(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentException("Invalid Id");
            }
            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == id.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException("Cannot find User");
            }
            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
                Quote = user.Quote,
                SubName = user.SubName,
                CreatedAt = user.CreatedAt,
            };
        }
        
        public async Task PostUserAsync(ApplicationUser user)
        {
            var profile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                AvatarUrl = "default",
                CreatedAt = DateTime.UtcNow,
            };
            _context.UserProfiles.Add(profile);
            await  _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentException("Invalid Id");
            }
            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException("Cannot find user");
            }

            return true;

        }
    }
}

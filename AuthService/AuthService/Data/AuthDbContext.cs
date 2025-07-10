using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthService.Models;

namespace AuthService.Data
{
    public class AuthDbContext : IdentityDbContext<UserEntity>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options) {
        }
        public DbSet<RoleEntity> Roles { get; set; }
    }
}

using AuthService.Domain.Aggregates;
using AuthService.Domain.Aggregates.ValueObjects;
using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using AuthService.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure
{
    public class AuthUserRepository : BaseRepository<AuthUser, AuthId>, IAuthUserRepository
    {

        public AuthUserRepository(AuthDbContext authDbContext) : base (authDbContext)
        {

        }
            
        public async Task<AuthUser> GetByName(string username)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<AuthUser?> GetByRefreshToken(string token)
        {
            return await _dbSet
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == token));
        }
    }
}

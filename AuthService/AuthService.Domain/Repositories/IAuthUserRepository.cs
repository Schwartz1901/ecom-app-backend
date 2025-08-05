using AuthService.Domain.Aggregates;
using AuthService.Domain.Aggregates.ValueObjects;
using AuthService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Repositories
{
    public interface IAuthUserRepository : IRepository<AuthUser, AuthId>
    {
        Task<AuthUser> GetByName(string username);
        Task<AuthUser?> GetByRefreshToken(string refreshToken);
        Task<AuthUser?> GetByEmail(string email);
    }
}

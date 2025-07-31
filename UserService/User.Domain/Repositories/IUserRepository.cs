using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Aggregates;
using User.Domain.Aggregates.ValueObjects;
using User.Domain.SeedWork;

namespace User.Domain.Repositories
{
    public interface IUserRepository : IRepository<UserAggregate, UserId>
    {
        Task<UserId> CreateUserAsync(UserAggregate user);
    }
}

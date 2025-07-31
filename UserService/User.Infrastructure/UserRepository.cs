using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Aggregates;
using User.Domain.Aggregates.ValueObjects;
using User.Domain.Repositories;
using User.Domain.SeedWork;
using User.Infrastructure.Repositories;

namespace User.Infrastructure
{
    public class UserRepository : BaseRepository<UserAggregate, UserId>, IUserRepository
    {
        public UserRepository(UserDbContext dbContext) : base(dbContext) { }
    }
}

using Cart.Domain.Aggregates.ValueObjects;
using Cart.Domain.Repositories;
using Order.Domain.Aggregates;
using Order.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Infrastructure
{
    public class CartRepository : BaseRepository<CartAggregate, CartUserId>, ICartRepository
    {
        public CartRepository(CartDbContext dbContext) : base(dbContext) { }


    }
}

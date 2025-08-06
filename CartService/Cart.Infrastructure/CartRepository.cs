using Cart.Domain.Aggregates.ValueObjects;
using Cart.Domain.Repositories;
using Cart.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure
{
    public class CartRepository : BaseRepository<CartAggregate, CartUserId>, ICartRepository
    {
        public CartRepository(CartDbContext dbContext) : base(dbContext) { }

        public override async Task<CartAggregate?> GetByIdAsync(CartUserId id) 
        { 
            var cart = await _dbSet.FirstOrDefaultAsync(c => c.CartUserId == id);
            return cart;
        }
    }
}

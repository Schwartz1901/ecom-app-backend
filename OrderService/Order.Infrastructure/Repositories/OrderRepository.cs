using Microsoft.EntityFrameworkCore;
using Order.Domain.Aggregates;
using Order.Domain.Aggregates.Entities;
using Order.Domain.Aggregates.Enumerations;
using Order.Domain.Aggregates.ValueObjects;
using Order.Domain.Repositories;
using Order.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<OrderAggregate, OrderId>, IOrderRepository
    {
        public OrderRepository (OrderDbContext dbContext) : base (dbContext) { }
        public override async Task<OrderAggregate?> GetByIdAsync(OrderId id)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        
       
    }
}

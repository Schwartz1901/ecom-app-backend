using Order.Domain.Aggregates;
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
        public async Task SubmitOrder(Cart cart, Buyer buyer, string description = "New Order") 
        {
            var itemList = cart.CartItems.Select(i => i.ToOrderItem()).ToList();
            var order = new OrderAggregate(buyer.Id, buyer.Address, DateTime.UtcNow, description, OrderStatus.Submitted, itemList);

            await _dbSet.AddAsync(order);
        }
    }
}

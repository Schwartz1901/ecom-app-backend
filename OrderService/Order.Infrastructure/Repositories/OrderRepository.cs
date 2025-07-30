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
       
    }
}

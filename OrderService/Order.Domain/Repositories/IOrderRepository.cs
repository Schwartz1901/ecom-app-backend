using Order.Domain.Aggregates;
using Order.Domain.Aggregates.ValueObjects;
using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Repositories
{
    public interface IOrderRepository: IRepository<OrderAggregate, OrderId>
    {
        Task SubmitOrder(Cart cart, Buyer buyer, string description);
    }
}

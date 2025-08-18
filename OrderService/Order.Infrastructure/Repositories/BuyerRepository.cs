using Order.Domain.Aggregates;
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
    public class BuyerRepository : BaseRepository<Buyer, BuyerId>, IBuyerRepository
    {
        public BuyerRepository(OrderDbContext context) : base(context) { }
    }
}

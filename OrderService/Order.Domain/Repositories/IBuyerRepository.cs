using Order.Domain.Aggregates;
using Order.Domain.Aggregates.ValueObjects;
using Order.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Repositories
{
    public interface IBuyerRepository: IRepository<Buyer, BuyerId>
    {
    }
}

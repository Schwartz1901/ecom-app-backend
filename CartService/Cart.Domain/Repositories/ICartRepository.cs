using Cart.Domain.Aggregates.ValueObjects;
using Cart.Domain.Aggregates;
using Cart.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Domain.Repositories
{
    public interface ICartRepository: IRepository<CartAggregate, CartUserId>
    {
        
    }
}

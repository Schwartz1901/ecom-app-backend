using Product.Domain.Aggregates;
using Product.Domain.Aggregates.ValueObjects;
using Product.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Repositories
{
    public interface IProductRepository : IRepository<ProductAggregate>
    {
        Task<ProductAggregate> GetByIdAsync(ProductId id);
        Task<ProductAggregate> GetByNameAsync(string name);
        Task AddAsync(ProductAggregate product);
        Task UpdateAsync(ProductAggregate product);
        
        
    }
}

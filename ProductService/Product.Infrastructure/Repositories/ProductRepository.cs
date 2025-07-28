using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Aggregates;
using Product.Domain.Aggregates.ValueObjects;
using Product.Domain.Repositories;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _dbContext;
        public ProductRepository(ProductDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<ProductAggregate> GetByIdAsync(ProductId id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            
            return product;
        }
        public async Task<ProductAggregate> GetByNameAsync(string name)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == name);
            
            return product;
        }
        public async Task AddAsync(ProductAggregate product)
        {
            
            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();

        }

        public async Task UpdateAsync(ProductAggregate product)
        {
            var exist = await _dbContext.Products.AnyAsync(p => p.Id == product.Id);

            if (!exist)
            {
                throw new Exception("Cannot Find Product");
            }

            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();

         

        }
    }
}

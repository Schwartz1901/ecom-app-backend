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

        public async Task<ProductAggregate?> GetByIdAsync(ProductId id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            
            return product;
        }
        public async Task<ProductAggregate?> GetByNameAsync(string name)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == name);
            
            return product;
        }
        public async Task<List<ProductAggregate>> GetAllAsync()
        {
            var products = await _dbContext.Products.ToListAsync();
            return products;
        }
        public async Task AddAsync(ProductAggregate product)
        {
            
            _dbContext.Add(product);
            //foreach (var category in product.Categories)
            //{
            //    _dbContext.Attach(category); // Tell EF this is an existing entity
            //}
            await _dbContext.SaveChangesAsync();

        }

        public async Task<bool> UpdateAsync(ProductAggregate product)
        {
            var exists = await _dbContext.Products.AnyAsync(p => p.Id == product.Id);
            if (!exists)
                throw new InvalidOperationException("Product not found.");

            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();
            return exists;

        }

        public async Task RemoveAsync(ProductId id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                _dbContext.Remove(product);
                await _dbContext.SaveChangesAsync();

            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

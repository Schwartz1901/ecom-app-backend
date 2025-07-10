using DocumentAPI.Controllers.DTOs;
using DocumentAPI.Interfaces;
using DocumentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly DocumentDbContext _context;

        public ProductService(DocumentDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductEntity>> GetAllAsync()
        {
            return await _context.ProductEntities.ToListAsync();
        }

        public async Task<ProductEntity?> GetByIdAsync(int id)
        {
            return await _context.ProductEntities.FindAsync(id);
        }

        public async Task<ProductEntity> AddAsync(ProductDto product)
        {
            var newProduct = new ProductEntity
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
            };
            _context.ProductEntities.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.ProductEntities.FindAsync(id);
            if (product == null) return false;

            _context.ProductEntities.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductEntity>> SearchAsync(string name)
        {
            var products = await _context.ProductEntities
                            .Where(p => p.Name.Contains(name)).ToListAsync();
            return products;
        }
    }
}

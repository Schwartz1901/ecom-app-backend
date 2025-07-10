using DocumentAPI.Interfaces;
using DocumentAPI.Models;
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

        public async Task AddAsync(ProductEntity product)
        {
            _context.ProductEntities.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.ProductEntities.FindAsync(id);
            if (product == null) return false;

            _context.ProductEntities.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

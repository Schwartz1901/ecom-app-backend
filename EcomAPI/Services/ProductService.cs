using AuthAPI;
using EcomAPI.Interfaces;


using EcomAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcomAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var products = await _context.Products.ToListAsync();

            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Cannot find Product with Id: {id}");
            }
            return product;
        }
    }
}

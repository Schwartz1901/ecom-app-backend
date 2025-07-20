using DocumentAPI.DTOs;
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
            if (id < 0) throw new ArgumentException("Invalid Id");
            return await _context.ProductEntities.FindAsync(id);
        }

        public async Task<ProductEntity> AddAsync(ProductDto productDto)
        {
            if (string.IsNullOrWhiteSpace(productDto.Name))
            {
                throw new ArgumentException("Product name is required!");
            }
            if (productDto.Price < 0)
            {
                throw new ArgumentException("Product price must be greater than or equal to 0");
            }
            // For testing purpose, to be removed later
            if (productDto.Price >= 100)
            {
                throw new ArgumentException("Too Expensive!!!!");
            }
            var newProduct = new ProductEntity
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
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
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
                throw new ArgumentException("Length of name must be greater than 2");
            var products = await _context.ProductEntities
                            .Where(p => p.Name.Contains(name)).ToListAsync();
            return products;
        }

        public async Task<ProductEntity> UpdateAsync(int id, ProductDto productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Unknown product!");
            }
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), "Invalid ID number!");
            }
            var product = await _context.ProductEntities.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} was not found.");
            }
            product.Name= productDto.Name;
            product.Description= productDto.Description;
            product.Price= productDto.Price;

            await _context.SaveChangesAsync();
            return product;
        }
        
 

    }
}

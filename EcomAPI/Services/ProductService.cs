using AuthAPI;
using EcomAPI.Controllers.Dtos;
using EcomAPI.Interfaces;


using EcomAPI.Models;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<Product> AddProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Id = productDto.Id,
                Catagory = productDto.Catagory,
                Name = productDto.Name,
                Price = productDto.Price,
                DiscountPrice = productDto.DiscountPrice,
                IsDiscount = productDto.IsDiscount,
                ImageUrl = productDto.ImageUrl,
                ImageAlt = productDto.ImageAlt,
                Description = productDto.Description
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null )
            {
                throw new KeyNotFoundException($"Cannot find product with Id: {id}");
            }

            _context.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product> UpdateProductAsync(int id, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Cannot find product with Id: {id}");

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.IsDiscount = productDto.IsDiscount;
            product.DiscountPrice = productDto.DiscountPrice;
            product.ImageUrl = productDto.ImageUrl;
            product.ImageAlt = productDto.ImageAlt;
            product.Catagory = productDto.Catagory;

            await _context.SaveChangesAsync();

            return product;
        }
    }
}

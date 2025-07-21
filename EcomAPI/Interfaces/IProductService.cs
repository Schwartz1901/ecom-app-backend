using EcomAPI.Controllers.Dtos;
using EcomAPI.Models;

namespace EcomAPI.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(ProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<Product> UpdateProductAsync(int id, ProductDto productDto);
    }
}

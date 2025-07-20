using EcomAPI.Models;

namespace EcomAPI.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync();
        Task<bool> DeleteProductAsync();
        Task<Product> UpdateProductAsync(Product product);
    }
}

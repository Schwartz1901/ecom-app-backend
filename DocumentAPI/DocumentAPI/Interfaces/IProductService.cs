
using DocumentAPI.Controllers.DTOs;
using DocumentAPI.Models;

namespace DocumentAPI.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductEntity>> GetAllAsync();
        Task<ProductEntity?> GetByIdAsync(int id);
        Task<ProductEntity> AddAsync(ProductDto product);
        Task<bool> DeleteAsync(int id);
        Task<List<ProductEntity>> SearchAsync(string name);
        Task<ProductEntity> UpdateAsync(int id, ProductDto product);
    }
}

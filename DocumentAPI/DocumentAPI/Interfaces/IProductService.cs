
using DocumentAPI.Models;

namespace DocumentAPI.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductEntity>> GetAllAsync();
        Task<ProductEntity?> GetByIdAsync(int id);
        Task AddAsync(ProductEntity product);
        Task<bool> DeleteAsync(int id);
    }
}

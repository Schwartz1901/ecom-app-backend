using Product.API.DTOs;

namespace Product.API.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(Guid Id);
        Task<List<ProductDto>> GetAllAsync();
        Task AddAsync(AddProductDto productDto);
        Task UpdateAsync(Guid id, UpdateProductDto productDto);
        Task DeleteAsync(Guid Id);
    }
}

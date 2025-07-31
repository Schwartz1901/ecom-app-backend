using Cart.API.DTOs;

namespace Cart.API.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetByIdAsync(Guid id);

        Task UpdateAsync(CartDto cart);
        Task DeleteAsync(Guid id);
    }
}

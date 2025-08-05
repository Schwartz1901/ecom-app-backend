using Cart.API.DTOs;

namespace Cart.API.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Guid id);
        Task UpdateAsync(CartDto cart);
        Task DeleteAsync(Guid id);
        Task AddItemToCartAsync(string authId, AddCartItemDto dto);
    }
}

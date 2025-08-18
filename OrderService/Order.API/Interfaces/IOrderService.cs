using Order.API.DTOs;

namespace Order.API.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetByIdAsync(Guid id);
        Task<List<OrderDto>> GetAllAsync();
        Task<List<OrderDto>> GetHistoryAsync(string id);
        Task AddAsync(OrderDto orderDto);
        Task UpdateAsync(OrderDto orderDto);
        Task DeleteAsync(Guid id);

        Task CheckoutAsync(string userId, AddressDto addressDto, string description, string name);

        
    }
}

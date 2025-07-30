using Order.API.DTOs;

namespace Order.API.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetByIdAsync(Guid id);
        Task<List<OrderDto>> GetAllAsync();
        Task AddAsync(OrderDto orderDto);
        Task UpdateAsync(OrderDto orderDto);
        Task DeleteAsync(Guid id);
    }
}

using Order.API.DTOs;

namespace Order.API.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetById(Guid id);
        Task<List<OrderDto>> GetAll();
        Task Add(OrderDto orderDto);
        Task Update(OrderDto orderDto);
        Task Delete(Guid id);
    }
}
